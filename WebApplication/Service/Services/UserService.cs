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
        public UserDto AddItem(UserDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            List<UserDto> users = new List<UserDto>();
            users = GetAll();
            foreach (UserDto user in users)
            {
                if (user.Email == item.Email)
                    throw new Exception("The email is already exists");
            }
            return _mapper.Map<UserDto>(_repository.AddItem(_mapper.Map<User>(item)));
        }
        public void DeleteItem(int id)
        {
            User user = _repository.GetById(id);
            if (user == null) throw new ArgumentNullException(nameof(id));
            user.IsActive = false;
            _repository.UpdateItem(user.Id, user);

        }
        public List<UserDto> GetAll()
        {
            return _mapper.Map<List<UserDto>>(_repository.GetAll());
        }
        public UserDto GetById(int id)
        {
            return _mapper.Map<UserDto>(_repository.GetById(id));
        }
        public void UpdateItem(int id, UserDto item)
        {
            User user = GetById(id);
            if (user != null)
            {

                user.NameUser = item.NameUser;
                user.Email = item.Email;
                user.Password = item.Password;
                _context.Save();
            }
            else
            {
                throw new ArgumentNullException(nameof(id));
            }
            _repository.UpdateItem(id, _mapper.Map<User>(item));
        }
    }
}