using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel_v1
{
    interface ICard
    {
        string TextureSourceFileName { get; }
        int CoordinateY { get; }
        int CoordinateX { get; }
        string Name { get; }
        int Id { get; }
    }
}
