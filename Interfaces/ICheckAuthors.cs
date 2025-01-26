using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book_Management.Contexts;
using Book_Management.Models;

namespace Book_Management.Interfaces
{
    public interface ICheckAuthors
    {
        public Author? CheckAuthor(int id);
    }
}
