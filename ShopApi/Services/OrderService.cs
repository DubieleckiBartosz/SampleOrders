using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopApi.AppDatabase;
using ShopApi.Dto.Orders;
using ShopApi.Entities;
using ShopApi.Interfaces;
using ShopApi.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using ShopApi.Email;
using ShopApi.Enums;
using ShopApi.Exceptions;
using ShopApi.Helpers;
using ShopApi.Models;
using ShopApi.QueryParameters;

namespace ShopApi.Services
{
    public class OrderService :BaseContextService<Order>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;
        private readonly IEmailService _emailService;
        private readonly ICsvExporter _csvExporter;
        public OrderService(ApplicationDbContext db,IMapper mapper,
            IShopService shopService,IEmailService emailService,
            ICsvExporter csvExporter) :base(db)
        {
            _csvExporter = csvExporter;
            _emailService = emailService;
            _shopService = shopService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<string>> CreateOrderAsync(Guid shopId,CreateOrderDto createOrder)
        {
            Dictionary<Guid,Product> productsDct;
            List<string> errorList = new();
            (bool exist, Shop shop) = await _shopService.ShopExistAsync(shopId);
            if (createOrder is null)
            {
                return BaseResponse<string>.Error(BaseResponseStrings.OrderNull);
            }
            else if (!exist)
            {
                return BaseResponse<string>.Error(BaseResponseStrings.DataNotFound($"Shop {shopId} "));
            }
            else
            {
                productsDct = await _db.Products.Where(w => w.ShopId == shopId)
                    .ToDictionaryAsync(d=>d.Id,v=>v);
                if (productsDct == null)
                {
                    return BaseResponse<string>.Error(BaseResponseStrings.ProductsNotFound);
                }
                  var checkProd=createOrder.Line.All(w => productsDct.ContainsKey(w.ProductId)); 
                  if (!checkProd)
                  {
                      return BaseResponse<string>.Error(BaseResponseStrings.IncorrectData);
                  }

                  
                  foreach (var itemOrderLineDto in createOrder.Line)
                  {
                      if (productsDct.TryGetValue(itemOrderLineDto.ProductId, out Product product))
                      {
                          if (itemOrderLineDto.Quantity > product.Quantity)
                          {
                             errorList.Add($"{itemOrderLineDto.ProductId} : Quantity available: {product.Quantity}");
                          }
                      }
                  }
                  if (errorList.Count != 0)
                  {
                      await CreateEmail(false, createOrder.Email,
                          null, shop.ShopName
                          ,OrderStatus.Declined.ToString(),
                          null,errorList);
                   
                      return BaseResponse<string>.Error(BaseResponseStrings.IncorrectData, errorList);
                  }
            }
            //Mogę zrobić dodatkowo else na górze w pętli i jakiś continue jeśli będzie błąd
            //i w tym momencie nie muszę pisać dwa razy tej samej pętli,
            //ale dla większej czytelności odseparuje to
            foreach (var item in createOrder.Line)
            {
                if (productsDct.TryGetValue(item.ProductId, out Product prdValue))
                {
                    prdValue.Quantity = (prdValue.Quantity - item.Quantity);
                }
            }
            createOrder.ShopId = shopId;
            var order = _mapper.Map<Order>(createOrder);
            order.Status = OrderStatus.Accepted.ToString();
            order.IsConfirmed = false;
            order .SummaryPrice = order.GetSummaryPriceOrder(productsDct);
            var orderCreated = await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            await CreateEmail(true, order?.Email,
                orderCreated.Entity.Id.ToString(),
                shop.ShopName, order.Status,
                order.SummaryPrice,errorList);
            
            return BaseResponse<string>.Ok($"Order ID: {orderCreated.Entity.Id.ToString()}, e-mail sent ");
        }

        public async Task<BaseResponse<string>> DeleteOrderAsync(Guid orderId)
        {
            var order =await _db.Orders.FirstOrDefaultAsync(s => s.Id == orderId);
            if(order is null)
            {
                return BaseResponse<string>.Error(BaseResponseStrings.DataNotFound(orderId));
            }
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return BaseResponse<string>.Ok(BaseResponseStrings.RequestSuccess);
        }

        public async Task<File> GetOrdersByDataAsync(CsvQuery query)
        {
            var orders = _db.Orders as IQueryable<Order>;

            orders = orders.Where(w => (w.CreatedDate == query.DateTo)
            && (w.CreatedDate >= query.DateFrom)
            && (w.SummaryPrice <= query.MaxSumPrice)
            && (w.SummaryPrice >= query.MinSumPrice));

            var status = query.Status.ToString();
            if (!string.IsNullOrEmpty(status))
            {
                switch ((int)query.Status)
                {
                    case 0:
                        orders =orders.Where(w =>
                            w.Status == status );
                        break;
                    case 1:
                        orders = orders.Where(w =>
                            w.Status == status);
                        break;
                    case 3:
                        orders = orders.Where(w =>
                            w.Status == status);
                        break;
                    default:
                        throw new ShopException("Incorrect status", HttpStatusCode.BadRequest);
                }
            }
            return await GetFileCSV(orders);
        }
  
        private async Task<File> GetFileCSV(IQueryable<Order> orders)
        {
            var orderList = await orders.ToListAsync();
            if (!orderList.Any())
            {
                throw new ShopException("List of orders is empty", HttpStatusCode.NotFound);
            }

            var modelToFile = _mapper.Map<IEnumerable<CsvModel>>(orderList);
            var result= _csvExporter.GetToCsvExport<CsvModel>(modelToFile);
            return new File
            {
                ContentType = "text/csv",
                FileName = "Zamówienia",
                Body = result
            };
        }
        private async Task CreateEmail(bool result,string emailTo,
            string orderId,string from,string status,decimal? summaryPrice,List<string> errorList)
        {
            if (errorList.Count> 0)
            {
                string bodyError = "";
                foreach (var item in errorList)
                {
                    bodyError += $"<li>{item}</li>";
                }
                var model = new EmailModel()
                {
                    To = emailTo,
                    Subject = "Twoje Zamówienie",
                    From = "Barti",
                    Body = $@"<h3>Witamy w {from}!</h3>" +
                           $"<p>Obecny status zamówienia: {status}</p><br/>" +
                           $"<p>Niestety, ale nie udało się zrealizować zamówienia<br/>" +
                           $"Przyczyna: <ul>{bodyError}</ul><p/>"
                };
                await SendEmail(model);
            }

            if (result)
            {
                var route = "api/v1/order/confirm-order/";
                var _endpointUri = new Uri(string.Concat($"https://localhost:5002/", route));
                var confirmUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "order", orderId);

                var model = new EmailModel()
                {
                    Body = $@"<h3>Witamy w {from}! </h3>" +
                           $"<p>Dziękujemy za wybranie naszego sklepu. <p/><br/>" +
                           $"Numer zamówienia: {orderId}<br/>"+
                           $"Status zamówienia: {status}<br/>" +
                           $"Kwota podsumowująca twoje zamówienie: {summaryPrice}<br/>"+
                           $"Prosimy o kliklnięcie linku w celu potwierdzenia zamówienia" +
                           $" <a href={confirmUri}>Link</a><p/>",
                    To = emailTo,
                    Subject = "Twoje Zamówienie",
                    From = "Barti"
                }; 
                await SendEmail(model);
            }
        }

        public async Task<BaseResponse<string>> ConfirmOrderAsync(string orderId)
        {
            var order=await _db.Orders.Where(w => w.Id == Guid.Parse(orderId))
                .FirstOrDefaultAsync();
         
            if (order!=null && 
                order.Status==OrderStatus.Accepted.ToString()&& 
                !order.IsConfirmed)
            {
                order.Status = OrderStatus.InProgress.ToString();
                order.IsConfirmed = true;
                _db.Update(order);
                await _db.SaveChangesAsync();
                return BaseResponse<string>.Ok(orderId,"Twoje zamówienie zostało przekazane do realizacji");
            }

            if (order.Status == OrderStatus.InProgress.ToString())
            {
                return BaseResponse<string>.Ok(orderId,"Twoje zamówienie zostało już potwierdzone" +
                                                      " i jest w trakcie realizacji");
            }
            return BaseResponse<string>.Error("Coś poszło nie tak");
        }

        private async Task<bool> SendEmail(EmailModel emailModel)
        {
           var result=
               await _emailService.SendEmailAsync(emailModel);
           if (result.Success)
           {
               return true;
           }
           return false;
        }
    }
}
