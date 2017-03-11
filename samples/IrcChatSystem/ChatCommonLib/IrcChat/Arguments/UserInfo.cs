using System;
using System.Security;

namespace Hik.Samples.Scs.IrcChat.Arguments
{
    /// <summary>
    /// Represents a chat user.
    /// This object particularly used in Login of a user.
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// Nick of user.
        /// </summary>
        public string Nick { get; set; }
        //public string IpAddress { get; set; }
        public string ComputerName { get; set; }
        public string OSVersion { get; set; }

        /// <summary>
        /// Bytes of avatar of user.
        /// </summary>
        public byte[] AvatarBytes { get; set; }

        /// <summary>
        /// Status of user.
        /// </summary>
        public UserStatus Status { get; set; }


        ///// <summary>
        ///// Secure Password
        ///// </summary>

        public string UserName { get; set; }
        public string SecurePassword { get; set; }

        //private SecureString securePassword;

        //public SecureString SecurePassword
        //{
        //    get { return securePassword; }
        //    set { securePassword = value; }
        //}
    }
}
