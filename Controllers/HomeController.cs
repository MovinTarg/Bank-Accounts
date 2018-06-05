using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank_Accounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bank_Accounts.Controllers
{
    public class HomeController : Controller
    {
        private BankAccountContext _bankAccountContext;
        public HomeController (BankAccountContext context)
        {
            _bankAccountContext = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }
        [Route("/signin")]
        public IActionResult Signin()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        [Route("/account")]
        public IActionResult Account()
        {
            User ActiveUser = _bankAccountContext.users
                .Include(User => User.Transactions)
                .Where(User => User.Id == HttpContext.Session.GetInt32("ActiveUserId"))
                .SingleOrDefault();
            if (ActiveUser.Transactions != null)
            {
                ActiveUser.Transactions = ActiveUser.Transactions.OrderByDescending(Transaction => Transaction.CreatedAt).ToList();
            }
            ViewBag.ActiveUser = ActiveUser;
            return View("Account");
        }
        [HttpPost]
        [Route("/user/register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User EmailCheck = _bankAccountContext.users.SingleOrDefault(User => User.Email == model.Email);
                if (EmailCheck == null)
                {
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        AccountBalance = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _bankAccountContext.Add(newUser);
                    _bankAccountContext.SaveChanges();
                    int ActiveUserId = _bankAccountContext.users.Last().Id;
                    HttpContext.Session.SetInt32("ActiveUserId", ActiveUserId);
                    return RedirectToAction("Account");
                }
                else
                {
                    ViewBag.Messages = "Email Taken!";
                }
            }
            return View("Index");
        }
        [Route("/user/login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User ActiveUser = _bankAccountContext.users.SingleOrDefault(User => User.Email == model.Email);
                if(ActiveUser != null)
                {
                    if(model.Password == ActiveUser.Password)
                    {
                        HttpContext.Session.SetInt32("ActiveUserId", ActiveUser.Id);
                        return RedirectToAction("Account");
                    }
                    else
                    {
                        ViewBag.Messages = "Incorrect Email / Password";
                    }
                }
            }
            return View("Login");
        }
        [Route("/account/transaction")]
        public IActionResult Transaction(Transaction model)
        {
            if (ModelState.IsValid)
            {
                int? ActiveUserId = HttpContext.Session.GetInt32("ActiveUserId");
                User User = _bankAccountContext.users.Where(user => user.Id == ActiveUserId).SingleOrDefault();
                if (model.Amount < 0 && ((model.Amount * -1) > User.AccountBalance))
                {
                    ViewBag.Messages = "Insufficient Funds";
                }    
                else
                {
                    Transaction newTransaction = new Transaction
                    {
                        Amount = model.Amount,
                        User = User,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    _bankAccountContext.Add(newTransaction);
                    User.AccountBalance += model.Amount;
                    _bankAccountContext.SaveChanges();
                    return RedirectToAction("Account");
                }
            }
            User ActiveUser = _bankAccountContext.users
                .Include(User => User.Transactions)
                .Where(User => User.Id == HttpContext.Session.GetInt32("ActiveUserId"))
                .SingleOrDefault();
            if (ActiveUser.Transactions != null)
            {
                ActiveUser.Transactions = ActiveUser.Transactions.OrderByDescending(Transaction => Transaction.CreatedAt).ToList();
            }
            ViewBag.ActiveUser = ActiveUser;
            return View("Account");
        }
    }
}
