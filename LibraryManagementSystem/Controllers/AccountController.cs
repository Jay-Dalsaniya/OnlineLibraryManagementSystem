using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailSender _emailSender;

    public AccountController(ApplicationDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user, IFormFile profileImage, string role)
    {
       

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());

        //if (existingUser != null)
        //{
        //    ModelState.AddModelError("Email", "An account with this email already exists.");
        //    return View(user);
        //}
        if (existingUser != null)
        {
            if (existingUser.IsActive == false)
            {
                ModelState.AddModelError("Email", "This email is already associated with a deactivated account. Please contact support.");
                return View(user);
            }
            else
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(user);
            }
        }

        user.CreatedDate = DateTime.UtcNow;

        if (string.IsNullOrEmpty(role) || (role != "Reader" && role != "Librarian" && role != "Admin"))
        {
            ModelState.AddModelError("Role", "Please select a valid role (Reader, Librarian, or Admin).");
            return View(user);
        }
        user.Role = role;

        if (profileImage != null && profileImage.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(profileImage.FileName);
            var extension = Path.GetExtension(profileImage.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }
            user.ImageName = newFileName;
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Add(user);
        await _context.SaveChangesAsync();

        string subject = "🎉 Welcome to Our Library!";
        string message = $@"
<html>
<head>
    <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Roboto', sans-serif;
            background-color: #f4f8fb;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
            padding: 30px;
        }}
        h1 {{
            text-align: center;
            color: #2e86de;
        }}
        p {{
            font-size: 16px;
            line-height: 1.6;
        }}
        .highlight {{
            font-weight: bold;
            color: #2e86de;
        }}
        .button {{
            display: inline-block;
            margin-top: 20px;
            padding: 12px 20px;
            background-color: #2e86de;
            color: #fff;
            border-radius: 5px;
            text-decoration: none;
            font-weight: bold;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #888;
            margin-top: 40px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Welcome to Our Library 📚</h1>
        <p>Hi <strong>{user.FirstName} {user.LastName}</strong>,</p>
        <p>We're excited to have you on board! 🎉</p>
        <p>You have <span class='highlight'>successfully registered</span>. You can now log in with your email and password to explore our vast collection of books and manage your account with ease.</p>
        <p style='text-align:center;'>
            <a href='https://your-library-login-url.com' class='button'>Log In Now</a>
        </p>
        <p>If you have any questions or need help, feel free to reach out to our support team.</p>
        <div class='footer'>
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>";

        await _emailSender.SendEmailAsync(user.Email, subject, message);

        return RedirectToAction("Login", "Account");
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            if (user != null && user.IsActive && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Role)
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (user.Role == "Reader")
                {
                    return RedirectToAction("Dashboard", "Reader");
                }
                else if (user.Role == "Librarian")
                {
                    return RedirectToAction("Dashboard", "Librarian");
                }
                else if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role.");
                    return View(model);
                }
            }
            else if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your account has been deactivated by the admin.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
        }

        return View(model);
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        var firstName = User.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(firstName))
        {
            return RedirectToAction("Login", "Account");
        }

        ViewBag.WelcomeMessage = $"Welcome – {firstName}";

        return View();
    }


    [Authorize]
    public IActionResult Profile()
    {
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    [HttpGet]
    [Authorize]
    public IActionResult EditProfile()
    {
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> EditProfile(User updatedUser, IFormFile profileImage)
    {
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.Address = updatedUser.Address;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Gender = updatedUser.Gender;
        user.Birthday = updatedUser.Birthday;

        if (profileImage != null && profileImage.Length > 0)
        {
            var fileName = Path.GetFileNameWithoutExtension(profileImage.FileName);
            var extension = Path.GetExtension(profileImage.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            user.ImageName = newFileName;
        }

        _context.Update(user);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Profile updated successfully!";
        return RedirectToAction("Profile", "Account");
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "No account found with this email.");
            return View();
        }

        string resetCode = GenerateRandomResetCode();  
        string subject = "🔐 Password Reset Request";
        string message = $@"
<html>
<head>
    <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Roboto', sans-serif;
            background-color: #f6f9fc;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: #ffffff;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            padding: 30px;
        }}
        h2 {{
            color: #2e86de;
            text-align: center;
        }}
        p {{
            font-size: 16px;
            color: #333;
            line-height: 1.6;
        }}
        .code {{
            background-color: #e0f0ff;
            border: 1px dashed #2e86de;
            color: #2e86de;
            font-weight: bold;
            font-size: 20px;
            text-align: center;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
            letter-spacing: 2px;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #888;
            margin-top: 30px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Password Reset Code</h2>
        <p>Hello,<strong>{user.FirstName} {user.LastName}</strong></p>
        <p>We received a request to reset your password. Please use the code below to proceed:</p>
        <div class='code'>{resetCode}</div>
        <p>If you did not request a password reset, you can ignore this email.</p>
        <p>Thank you,<br/>The Library Team</p>
        <div class='footer'>
            <p>This is an automated email. Please do not reply.</p>
        </div>
    </div>
</body>
</html>";

        await _emailSender.SendEmailAsync(user.Email, subject, message);

        // Store the reset code and email in the session
        HttpContext.Session.SetString("ResetCode", resetCode);
        HttpContext.Session.SetString("Email", user.Email);

        return RedirectToAction("EnterResetCode");
    }

    [HttpGet]
    public IActionResult EnterResetCode()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EnterResetCode(string email, string resetCode)
    {
        string storedResetCode = HttpContext.Session.GetString("ResetCode");

        if (storedResetCode != resetCode)
        {
            ModelState.AddModelError(string.Empty, "The reset code is invalid. Please try again.");
            ViewBag.Email = email;
            return View();
        }

        return RedirectToAction("ResetPassword");
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
            return View();
        }

        string email = HttpContext.Session.GetString("Email");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        if (user == null)
        {
            return RedirectToAction("ForgotPassword");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _context.Update(user);
        await _context.SaveChangesAsync();

        string message = "Your password has been successfully changed.";
        await _emailSender.SendEmailAsync(user.Email, "Password Change Confirmation", message);

        TempData["SuccessMessage"] = "Password successfully changed!";
        return RedirectToAction("Login");
    }

    public string GenerateRandomResetCode()
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();  
        return code;
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        
            var userFirstName = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName.ToLower() == userFirstName.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return View(model);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your password has been successfully changed!";
            return RedirectToAction("Profile");
            }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    [AllowAnonymous] 
    public IActionResult AccessDenied()
    {
        return View();
    }
}
