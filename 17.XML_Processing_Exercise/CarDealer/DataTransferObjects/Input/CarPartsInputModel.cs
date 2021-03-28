using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects.Input
{
    [XmlType("partId")]
    public class CarPartsInputModel
    {
        [XmlAttribute]
        public int Id { get; set; }
    }
}