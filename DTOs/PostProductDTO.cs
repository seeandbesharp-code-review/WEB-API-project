using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
     public record PostProductDTO(
     int Id ,
     int CategoryId ,
     string Name ,
     string Description ,
     double Price ,
     string ImageUrl,
     bool IsAvailable = true
        );
  
}
