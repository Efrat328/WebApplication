using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Dto;
using Service.Interface;

namespace Service.Services
{
    public class UserService : IService<UserDto>
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<UserDto> AddItem(UserDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<UserDto> users = await GetAll();
            foreach (UserDto user in users)
            {
                if (user.Email == item.Email)
                    throw new Exception("The email is already exists");
                if (user.NameUser == item.NameUser)
                    throw new Exception("The name is already exists");
            }
            item.IsActive = true;  // ✅ הוסיפי את זה
            return _mapper.Map<UserDto>(await _repository.AddItem(_mapper.Map<User>(item)));
        }
        public async Task DeleteItem(int id)
        {
            User user = await _repository.GetById(id);
            if (user == null) throw new ArgumentNullException(nameof(id));
            user.IsActive = false;
            await _repository.UpdateItem( user);

        }
        public async Task<List<UserDto>> GetAll()
        {
            return _mapper.Map<List<UserDto>>(await _repository.GetAll());
        }
        public async Task<UserDto> GetById(int id)
        {
            return _mapper.Map<UserDto>(await _repository.GetById(id));
        }
        public async Task UpdateItem(int id, UserDto item)
        {
            User user = await _repository.GetById(id);
            if (user != null)
            {

                user.NameUser = item.NameUser;
                user.Email = item.Email;
                user.Password = item.Password;
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
            await _repository.UpdateItem( _mapper.Map<User>(user));
        }
    }
}