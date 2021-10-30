using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Helpers
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = string.Format("ایمیل {0} قبلا ثبت شده", email)
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format("نام کاربری {0} قبلا ثبت شده", userName)
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = string.Format("ایمیل وارد شده معتبر نیست", email)
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = string.Format("نقش {0} قبلا ثبت شده", role)
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = string.Format("نقش وارد شده معتبر نیست", role)
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "توکن معتبر نیست"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format("نام کاربری وارد شده معتبر نیست", userName)
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = "شما قبلا وارد شدید"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "عدم تطایق رمز عبور"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "رمز عبور باید شامل عدد باشد"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "رمز عبور باید شامل حرف کوچک باشد"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "رمز عبور باید شامل کارکتر خاص باشد"
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = string.Format("رمز عبور باید شامل {0} کارکتر خاص باشد", uniqueChars)
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "رمز عبور باید شامل حرف بزرگ باشد"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = string.Format("رمز عبور باید بیشتر از {0} حرف باشد", length)
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = "کاربر رمز غبور دارد"
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = string.Format("نقش {0} برای کاربر ثبت شده", role)
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = string.Format("نقش {0} برای کاربر ثبت نشده", role)
            };
        }

        //public override IdentityError UserLockoutNotEnabled()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(UserLockoutNotEnabled),
        //        Description = LocalizedIdentityErrorMessages.UserLockoutNotEnabled
        //    };
        //}

        //public override IdentityError RecoveryCodeRedemptionFailed()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(RecoveryCodeRedemptionFailed),
        //        Description = LocalizedIdentityErrorMessages.RecoveryCodeRedemptionFailed
        //    };
        //}

        //public override IdentityError ConcurrencyFailure()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(ConcurrencyFailure),
        //        Description = LocalizedIdentityErrorMessages.ConcurrencyFailure
        //    };
        //}

        //public override IdentityError DefaultError()
        //{
        //    return new IdentityError
        //    {
        //        Code = nameof(DefaultError),
        //        Description = LocalizedIdentityErrorMessages.DefaultIdentityError
        //    };
        //}
    }
}
