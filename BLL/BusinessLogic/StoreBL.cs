using BLL.DataTransferObjects;
using BLL.Interfaces;
using DAL.DataAccess;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pagination.DO;
using Pagination.Filter;
using Pagination.Helpers;
using Pagination.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.BusinessLogic
{
    public class StoreBL : IStoreBL
    {
        private IStoreDA storeDA = new StoreDA();

        public async Task<IActionResult> GetPage(PaginationFilter filter, HttpRequest request)
        {
            var route = request.Path.Value;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            IUriService uriService = new UriService(uri);
            PaginationFilter validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<Store> storeList = await storeDA.GetPage(validFilter.PageNumber, validFilter.PageSize);
            List<StoreDTO> storeDtoList = new List<StoreDTO>();

            foreach(Store store in storeList)
            {
                storeDtoList.Add(new StoreDTO
                {
                    Id = store.Id,
                    Store_Name = store.Store_Name,
                    Phone = store.Phone,
                    Email = store.Email,
                    Street = store.Street,
                    City = store.City,
                    Area = store.Area,
                    Zip_Code = store.Zip_Code
                });
            }

            int totalRecords = await storeDA.GetRecords();
            Information info = FillInformation(totalRecords, validFilter.PageNumber, validFilter.PageSize);

            var pagedResponse = PaginationHelper.CreatePagedReponse<StoreDTO>(
                storeDtoList, info, validFilter, totalRecords, uriService, route
                );

            return new OkObjectResult(pagedResponse);
        }

        public async Task<IActionResult> GetStore(long id)
        {
            ActionResult<Store> action = await storeDA.GetStore(id);

            if(action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            Store store = action.Value;
            StoreDTO storeDTO = new StoreDTO
            {
                Id = store.Id,
                Store_Name = store.Store_Name,
                Phone = store.Phone,
                Email = store.Email,
                Street = store.Street,
                City = store.City,
                Area = store.Area,
                Zip_Code = store.Zip_Code
            };

            return new OkObjectResult(storeDTO);
        }
        public async Task<IActionResult> PutStore(long id, StoreDTO storeDTO)
        {
            if(id != storeDTO.Id)
            {
                return new BadRequestResult();
            }

            /* Generate secret_info column
             * string secretInfo = "generateSecret";
             * Inside new Store object: Secret_Info = secretInfo
             * Currently secret_info is overrids by null
             */
            Store store = new Store
            {
                Id = storeDTO.Id,
                Store_Name = storeDTO.Store_Name,
                Phone = storeDTO.Phone,
                Email = storeDTO.Email,
                Street = storeDTO.Street,
                City = storeDTO.City,
                Area = storeDTO.Area,
                Zip_Code = storeDTO.Zip_Code,
                Secret_Info = null
            };

            try
            {
                await storeDA.PutStore(store);
            }
            catch(DbUpdateConcurrencyException) when (!storeDA.Exists(id))
            {
                return new NotFoundResult();
            }

            return new NoContentResult();
        }
        public async Task<ActionResult<StoreDTO>> PostStore(StoreDTO storeDTO)
        {
            /* Generate secret_info column
             * string secretInfo = "generateSecret";
             * Inside new Store object: Secret_Info = secretInfo
             * Currently secret_info is overrids by null
             */
            Store store = new Store
            {
                Store_Name = storeDTO.Store_Name,
                Phone = storeDTO.Phone,
                Email = storeDTO.Email,
                Street = storeDTO.Street,
                City = storeDTO.City,
                Area = storeDTO.Area,
                Zip_Code = storeDTO.Zip_Code,
                Secret_Info = null
            };

            await storeDA.PostStore(store);

            return new CreatedAtActionResult(nameof(GetStore), "Store", new {Id = store.Id}, storeDTO);
        }
        public async Task<IActionResult> DeleteStore(long id)
        {
            ActionResult<Store> action = await storeDA.GetStore(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }
            await storeDA.DeleteStore(id, action.Value);

            return new NoContentResult();
        }

        private Information FillInformation(int totalRecords, int pageNumber, int pageSize)
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
