using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class ImportCarsInputModel
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int TravalledDistance { get; set; }

        public IEnumerable<int> PartsId { get; set; }


    }
}
