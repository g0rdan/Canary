using System;
namespace Canary.SoC
{
    /// <summary>
    /// The class which shows processor usage statistics in percentages
    /// </summary>
    public class UsageInformation
    {
        // Represent amount of active properties.
        // Different platforms could have only 1 or 2 properties.
        int _activeProperties;

        float _common;
        public float Common { 
            get 
            {
                // In case if we have only common information on some platform
                if (_common > 0)
                    return _common;
                
                // Excluding Idle cause it represents "doing notnihg"
                return (User + System + Other) / _activeProperties;
            }
            set { _common = value; } 
        }

        float _user;
        public float User { 
            get { return _user; }
            set { 
                _user = value;
                _activeProperties++;
            }
        }

        float _system;
        public float System { 
            get { return _system; }
            set { 
                _system = value; 
                _activeProperties++;
            }
        }

        float _other;
        public float Other { 
            get { return _other; }
            set { 
                _other = value; 
                _activeProperties++;
            }
        }

        public float Idle { get; set; }
    }
}
