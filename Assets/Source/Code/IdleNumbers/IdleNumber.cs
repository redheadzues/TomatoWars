using System;

namespace Source.Code.IdleNumbers
{
        public struct IdleNumber
    {
        public float Value { get; private set; }
        public int Degree { get; private set; }

        private int _id;
        private const int _tenCubed = 1000;

        public IdleNumber(float value, int degree)
        {
            Value = value;
            Degree = degree;
            _id = Guid.NewGuid().GetHashCode();

            NormalizedNumber();
        }

        public IdleNumber(float value)
        {
            Value = value;
            Degree = 0;
            _id = Guid.NewGuid().GetHashCode();

            NormalizedNumber();
        }

        public IdleNumber(double value)
        {
            Value = (float)value;
            Degree = 0;
            _id = Guid.NewGuid().GetHashCode();

            NormalizedNumber();
        }

        private void NormalizedNumber()
        {
            if (Math.Abs(Value) < 1 && Degree != 0)
            {
                while (Math.Abs(Value) < 1)
                {
                    Value *= _tenCubed;
                    Degree -= 3;
                }
            }

            while (Math.Abs(Value) >= _tenCubed)
            {
                Value /= _tenCubed;
                Degree += 3;
            }
        }
                
        public static implicit operator IdleNumber(int value)
        {
            return new IdleNumber(value);
        }

        public static implicit operator IdleNumber(float value)
        {
            return new IdleNumber(value);
        }

        public static implicit operator IdleNumber(double value)
        {
            return new IdleNumber(value);
        }
        
        public static IdleNumber operator +(IdleNumber leftNumber, IdleNumber rightNumber)
        {

            if (leftNumber.Degree >= rightNumber.Degree)
            {
                int difference = leftNumber.Degree - rightNumber.Degree;
                float resultValue = leftNumber.Value + rightNumber.Value / (float)Math.Pow(10, difference);

                return new IdleNumber(resultValue, leftNumber.Degree);
            }
            else
            {
                int difference = rightNumber.Degree - leftNumber.Degree;
                float resultValue = rightNumber.Value + leftNumber.Value / (float)Math.Pow(10, difference);

                return new IdleNumber(resultValue, rightNumber.Degree);
            }
        }

        public static IdleNumber operator +(IdleNumber idleNumber, float f)
        {
            float resultValue = idleNumber.Value + f / (float)Math.Pow(10, idleNumber.Degree);

            return new IdleNumber(resultValue, idleNumber.Degree);
        }

        public static IdleNumber operator +(IdleNumber idleNumber, int i)
        {
            float resultValue = idleNumber.Value + i / (float)Math.Pow(10, idleNumber.Degree);

            return new IdleNumber(resultValue, idleNumber.Degree);
        }

        public static IdleNumber operator -(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            int difference = leftNumber.Degree - rightNumber.Degree;
            float resultValue = leftNumber.Value - rightNumber.Value / (float)Math.Pow(10, difference);

            return new IdleNumber(resultValue, leftNumber.Degree);
        }

        public static IdleNumber operator -(IdleNumber idleNumber, float f)
        {
            float resultValue = idleNumber.Value - f / (float)Math.Pow(10, idleNumber.Degree);

            return new IdleNumber(resultValue, idleNumber.Degree);
        }

        public static IdleNumber operator *(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            float resultValue = leftNumber.Value * rightNumber.Value;
            int resultDegree = leftNumber.Degree + rightNumber.Degree;

            return new IdleNumber(resultValue, resultDegree);
        }

        public static IdleNumber operator *(IdleNumber idle, float f)
        {
            return new IdleNumber(idle.Value * f, idle.Degree);
        }

        public static IdleNumber operator *(IdleNumber leftNumber, double d)
        {
            IdleNumber rightNumber = new(d);

            return leftNumber * rightNumber;
        }

        public static IdleNumber operator /(IdleNumber dividend, IdleNumber divider)
        {
            if(dividend.Value == 0 || divider.Value == 0)
                return new IdleNumber(0);

            float resultValue = dividend.Value / divider.Value;
            int resultDegree = dividend.Degree - divider.Degree;

            return new IdleNumber(resultValue, resultDegree);
        }

        public static IdleNumber operator /(IdleNumber divided, float f)
        {
            IdleNumber divider = new(f);

            return divided / divider;
        }

        public static bool operator >(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            if (leftNumber.Value >= 0 && rightNumber.Value < 0)
                return true;

            if (leftNumber.Value < 0 && rightNumber.Value >= 0)
                return false;
            
            
            if(leftNumber.Degree >= rightNumber.Degree)
            {
                if(leftNumber.Degree == rightNumber.Degree)
                {
                    return leftNumber.Value > rightNumber.Value;
                }

                return true;
            }

            return false;
        }

        public static bool operator >(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber > rightNumber;
        }

        public static bool operator <(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            if (leftNumber.Value < 0 && rightNumber.Value >= 0)
                return true;

            if (leftNumber.Value >= 0 && rightNumber.Value < 0)
                return false;
            
            if (leftNumber.Degree <= rightNumber.Degree)
            {
                if (leftNumber.Degree == rightNumber.Degree)
                {
                    return leftNumber.Value < rightNumber.Value;
                }

                return true;
            }

            return false;
        }

        public static bool operator <(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber < rightNumber;
        }

        public static bool operator >=(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            return !(leftNumber < rightNumber);
        }

        public static bool operator >=(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber >= rightNumber;
        }

        public static bool operator <=(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            return !(leftNumber > rightNumber);
        }

        public static bool operator <=(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber <= rightNumber;
        }

        public static bool operator ==(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            return leftNumber.Degree == rightNumber.Degree && leftNumber.Value == rightNumber.Value;
        }

        public static bool operator ==(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber == rightNumber;
        }

        public static bool operator !=(IdleNumber leftNumber, IdleNumber rightNumber)
        {
            return !(leftNumber == rightNumber);
        }

        public static bool operator !=(IdleNumber leftNumber, float f)
        {
            IdleNumber rightNumber = new(f);

            return leftNumber != rightNumber;
        }

        public override bool Equals(object obj)
        {
            IdleNumber idleNumber = (IdleNumber)(obj as IdleNumber?);
            if (idleNumber == null)
            {
                return false;
            }
            return _id == idleNumber.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public override string ToString()
        {
            string value;

            if (this > _tenCubed)
                value = Value.ToString("#.##");
            else
                value = Math.Round(Value).ToString();

            return value + " " + PowTenToName.Get(Degree);
        }
    }
}