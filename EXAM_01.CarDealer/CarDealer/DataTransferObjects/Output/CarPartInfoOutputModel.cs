using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Output
{
    [XmlType("parts")]
    public class CarPartInfoOutputModel 
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
