using Pagination.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagination.Helpers
{
    public class InformationHelper
    {
        public static Information FillInformation(int totalRecords, int pageNumber, int pageSize)
        {
            const string EMPTY_TABLE_ERR = "Empty table, Check the table in Database";
            const string PAGE_NOT_EXIST_ERR = "Page not exist, Page number is greater than total pages." +
                    " For more information check count of 'totalPages' in the response";
            var totalPages = ((double)totalRecords / (double)pageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            Information info = new Information();

            if (totalRecords == 0)
            {
                info.Error = EMPTY_TABLE_ERR;
            }
            else if (pageNumber > roundedTotalPages)
            {
                info.Error = PAGE_NOT_EXIST_ERR;
            }

            info.Succeeded = (info.Error == null);
            info.Message = info.Succeeded ? "Request is successful" : null;

            return info;
        }
    }
}
