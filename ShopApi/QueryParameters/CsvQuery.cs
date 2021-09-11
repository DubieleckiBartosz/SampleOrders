using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Enums;
using ShopApi.Exceptions;

namespace ShopApi.QueryParameters
{
    public class CsvQuery
    {
        private DateTime _baseDateFrom = DateTime.Now.AddMonths(-1);
        private DateTime _baseDateTo = DateTime.Now;
        private decimal _maxSumPrice = 1000;
        private decimal _minSumPrice=0;
        public DateTime DateFrom
        {
            get =>_baseDateFrom;
            set
            {
                _baseDateFrom =value == default?_baseDateTo.AddMonths(-1) :(value < _baseDateTo)
                    ? value
                    : _baseDateTo.Date.AddMonths(-1);
            }
        }
        public DateTime DateTo 
        {
            get=>_baseDateTo;
            set => _baseDateTo = value==default?_baseDateFrom.AddMonths(1):(value> _baseDateFrom)
            ?value
            : _baseDateFrom.AddMonths(1);
        }

        public OrderStatus Status { get; set; }

        public decimal MaxSumPrice
        {
            get=> _maxSumPrice;
            set => _maxSumPrice = (value > 5) ? value : _maxSumPrice;
        }

        public decimal MinSumPrice
        {
            get=>_minSumPrice;
            set=>_minSumPrice=(value>0)?value:_minSumPrice;
        }
    }
}
