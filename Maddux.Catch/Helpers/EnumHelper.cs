using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Maddux.Catch.Helpers
{
    public static class EnumHelper
    {
        public static void PopulateDropDownList<TEnum>(DropDownList dropDownList) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = ((int)(object)e).ToString()
            }).ToList();

            dropDownList.Items.Clear();
            dropDownList.Items.AddRange(values.Select(v => new ListItem(v.Text, v.Value)).ToArray());
        }
    }
}