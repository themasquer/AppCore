using System;
using System.Collections.Generic;
using AppCore.Utils;

namespace AppCore.Records.Abstracts
{
    public abstract class OrderPageRecordBase 
    {
        public int PageNumber { get; set; }
        public int RecordsCount { get; set; }
        public int RecordsPerPageCount { get; set; }
        public string OrderExpression { get; set; }
        public string OrderDirection { get; set; }
        public string Order => OrderExpression == null || OrderDirection == null ? null : OrderExpression + " " + OrderDirection;

        public List<int> PageNumbers
        {
            get
            {
                List<int> pages = new List<int>();
                if (RecordsCount > 0 && RecordsPerPageCount > 0)
                {
                    int numberOfPages = Convert.ToInt32(Math.Ceiling((decimal)RecordsCount / (decimal)RecordsPerPageCount));
                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                }
                return pages;
            }
        }

        protected OrderPageRecordBase()
        {
            PageNumber = 1;
            RecordsCount = 0;
            RecordsPerPageCount = 0;
        }

        public virtual void SetPageModel(int pageNumber, int recordCount, int recordsPerPageCount)
        {
            PageNumber = pageNumber;
            RecordsCount = recordCount;
            RecordsPerPageCount = recordsPerPageCount;
        }

        public virtual void SetOrderModel(string order)
        {
            if (!String.IsNullOrWhiteSpace(order))
            {
                string[] items = StringUtil.GetArrayByCharacter(order, ' ');
                if (items.Length >= 2)
                {
                    int index = items.Length - 1;
                    OrderDirection = items[index].ToUpper();
                    OrderExpression = "";
                    for (int i = 0; i < index; i++)
                    {
                        OrderExpression += items[i] + " ";
                    }
                    OrderExpression = OrderExpression.Trim();
                }
            }
        }
    }
}
