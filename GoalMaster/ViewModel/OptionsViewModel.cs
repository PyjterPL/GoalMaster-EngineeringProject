using GoalMaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.IO;
using GoalMaster.Properties;
using System.Drawing;

namespace GoalMaster.ViewModel
{
    class OptionsViewModel : MainViewModel
    {

        public RelayCommand UploadCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public event EventHandler CloseWindowEvent;

        private User _user;
        public OptionsViewModel(User user, bool isOwner = true)
        {
            UploadCommand = new RelayCommand(Upload);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            this.IsOwner = isOwner;
            this._user = user;
            var placeholderImage = Resources.no_image_available;
            UserImage = ImageToByte(placeholderImage);

            using (var db = new GoalMasterDatabaseContext())
            {
                var name = db.Users.FirstOrDefault(us => us.ID == this._user.ID).Login;
                var email = db.Users.FirstOrDefault(us => us.ID == this._user.ID).Mail;
                
                Name = name;
                Email = email;

                var userInfo = db.UsersInfo.FirstOrDefault(ui => ui.User.ID == _user.ID);
                if (userInfo == null)
                    return;

                var description = userInfo.UserDescription;
                var address = userInfo.Address;
                var image = userInfo.ProfileImage;
                var birthDate = userInfo.BirthDate;

                Description = description;
                Address = address;
                UserImage = image;
                if(UserImage==null)
                    UserImage = ImageToByte(placeholderImage);
                BirthDate = birthDate;
            }
        

        }

        private void Cancel()
        {
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, null);
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private void Save()
        {
            using (var db = new GoalMasterDatabaseContext())
            {
                var userInfo = db.UsersInfo.FirstOrDefault(ui => ui.User.ID == _user.ID);
                if (userInfo == null) //dodać nowe userInfo
                {
                    var newUserInfo = new UserInfo();
                    
                    newUserInfo.ProfileImage = UserImage;
                    newUserInfo.UserDescription = Description;
                    newUserInfo.Address = Address;
                    newUserInfo.BirthDate = BirthDate;
                    db.UsersInfo.Add(newUserInfo);
                    newUserInfo.User = db.Users.First(us => us.ID == _user.ID);
                    db.SaveChanges();
                }
                else //updateować userInfo
                {
                    userInfo.UserDescription = Description;
                    userInfo.Address = Address;
                    userInfo.ProfileImage = UserImage;
                    userInfo.BirthDate = BirthDate;
                    db.SaveChanges();
                }

                if (CloseWindowEvent != null)
                    CloseWindowEvent(this, null);
            }
        }

        private void Upload()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"; //use as you require.
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = openFileDialog1.FileNames;
                FileStream imageStream = new FileStream(fileNames[0], FileMode.Open, FileAccess.Read);
                UserImage = ReadFully(imageStream);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private byte[] userImage;

        public byte[] UserImage
        {
            get { return userImage; }
            set
            {
                if (value != userImage)
                {
                    userImage = value;
                    RaisePropertyChanged("UserImage");
                }
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged("Address");
            }
        }
        private DateTime _birthDate;

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value;
                RaisePropertyChanged("BirthDate");
            }
        }
        private bool _isOwner;

        public bool IsOwner
        {
            get { return _isOwner; }
            set { _isOwner = value;
                RaisePropertyChanged("IsOwner");
            }
        }


    }
}
