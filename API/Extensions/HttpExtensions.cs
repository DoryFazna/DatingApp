using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages){

            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            
            //To make the header value not in title case format, bt in camelcase
            var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));

            //Adding cors header onto this to make this header available
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
        
    }
}