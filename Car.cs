using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGBotNVK;

class Car
{
    public string Name = "";
    public string Number = "";
    public string Color = "";

    public override string ToString()
    {
        return $"Автомобиль: {Name}, номер: {Number}, цвет: {Color}";
    }
}
