﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartServer.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //Foreign Key relationship
        [ForeignKey("Cart")]
        public long CartId { get; set; }
        public string BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCollectionId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
