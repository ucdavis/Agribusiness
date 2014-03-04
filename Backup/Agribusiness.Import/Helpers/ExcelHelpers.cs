using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Agribusiness.Import.Helpers
{
	public static class ExcelHelpers
	{
        public static Sheet OpenWorkbook(string fileName)
        {
            var stream = new FileStream(fileName, FileMode.Open);
            var workbook = new HSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            return sheet;
        }

        public static string ReadCell(Row row, int index)
        {
            var val = row.GetCell(index);

            return val != null ? val.ToString() : null;
        }

        public static int? ReadIntCell(Row row, int index)
        {
            var val = row.GetCell(index);

            int num;
            if (val != null && int.TryParse(val.ToString(), out num))
            {
                return num;
            }

            return null;
        }

        public static DateTime? ReadDateCell(Row row, int index)
        {
            var val = row.GetCell(index);

            DateTime date;
            if (val != null && DateTime.TryParse(val.ToString(), out date))
            {
                return date;
            }

            return null;
        }

        public static bool? ReadBoolCell(Row row, int index)
        {
            var val = row.GetCell(index);

            if (val != null)
            {
                return val.ToString() == "yes" ? true : false;
            }

            return null;
        }
	}
}