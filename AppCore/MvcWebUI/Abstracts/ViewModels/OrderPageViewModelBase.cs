using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using AppCore.Records.Abstracts;

namespace AppCore.MvcWebUI.Abstracts.ViewModels
{
    public abstract class OrderPageViewModelBase : OrderPageRecordBase 
    {
        public virtual SelectList PageNumberSelectList => new SelectList(PageNumbers.Select(e => new SelectListItem()
        {
            Value = e.ToString(),
            Text = e.ToString()
        }), "Value", "Text", PageNumber);
    }
}
