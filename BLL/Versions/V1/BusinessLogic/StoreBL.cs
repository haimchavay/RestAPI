using BLL.Versions.V1.DataTransferObjects;
using BLL.Versions.V1.Interfaces;
using DAL.Versions.V1.DataAccess;
using DAL.Versions.V1.Entities;
using DAL.Versions.V1.Interfaces;
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

namespace BLL.Versions.V1.BusinessLogic
{
    public class StoreBL : IStoreBL
    {
        private readonly IStoreDA storeDA = new StoreDA();

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
                storeDtoList.Add(ItemToDTO(store));
            }

            int totalRecords = await storeDA.GetRecords();
            Information info = InformationHelper.FillInformation(totalRecords, validFilter.PageNumber, validFilter.PageSize);

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
            StoreDTO storeDTO = ItemToDTO(store);

            return new OkObjectResult(storeDTO);
        }
        public async Task<IActionResult> PutStore(long id, StoreDTO storeDTO)
        {
            if(id != storeDTO.Id)
            {
                return new BadRequestResult();
            }

            ActionResult<Store> action = await storeDA.GetStore(storeDTO.Id);
            if(action == null || action.Value == null)
            {
                return new NotFoundResult();
            }

            Store store = action.Value;
            store.Id = storeDTO.Id;
            store.Name = storeDTO.Name;
            store.Phone = storeDTO.Phone;
            store.Area = storeDTO.Area;
            store.City = storeDTO.City;
            store.Street = storeDTO.Street;
            store.ZipCode = storeDTO.ZipCode;
            store.ContactMan = storeDTO.ContactMan;
            store.ContactMan2 = storeDTO.ContactMan2;
            store.StoreTypeId = storeDTO.StoreTypeId;
            store.UserId = storeDTO.UserId;
            store.PhotoPath = storeDTO.PhotoPath;
            store.Description = storeDTO.Description;

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

            Store store = DtoToItem(storeDTO);

            await storeDA.PostStore(store);

            return new CreatedAtActionResult(nameof(GetStore), "Stores", new {Id = store.Id}, ItemToDTO(store));
        }
        public async Task<IActionResult> DeleteStore(long id)
        {
            ActionResult<Store> action = await storeDA.GetStore(id);

            if (action == null || action.Value == null)
            {
                return new NotFoundResult();
            }
            await storeDA.DeleteStore(action.Value);

            return new NoContentResult();
        }
        private static StoreDTO ItemToDTO(Store store) =>
            new StoreDTO
            {
                Id = store.Id,
                Name = store.Name,
                Phone = store.Phone,
                Area = store.Area,
                City = store.City,
                Street = store.Street,
                ZipCode = store.ZipCode,
                ContactMan = store.ContactMan,
                ContactMan2 = store.ContactMan2,
                StoreTypeId = store.StoreTypeId,
                UserId = store.UserId,
                PhotoPath = store.PhotoPath,
                Description = store.Description
            };
        private static Store DtoToItem(StoreDTO storeDTO) =>
            new Store
            {
                Id = storeDTO.Id,
                Name = storeDTO.Name,
                Phone = storeDTO.Phone,
                Area = storeDTO.Area,
                City = storeDTO.City,
                Street = storeDTO.Street,
                ZipCode = storeDTO.ZipCode,
                ContactMan = storeDTO.ContactMan,
                ContactMan2 = storeDTO.ContactMan2,
                StoreTypeId = storeDTO.StoreTypeId,
                UserId = storeDTO.UserId,
                PhotoPath = storeDTO.PhotoPath,
                Description = storeDTO.Description
            };
    }
}
