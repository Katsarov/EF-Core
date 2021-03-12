using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        //[EmailAddress] - автоматично валидира мейл адреса
        public string Email { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Bet> Bets { get; set; }

        // ICollection<bets>
    }


    // UserId, Username, Password, Email, Name, Balance
}
