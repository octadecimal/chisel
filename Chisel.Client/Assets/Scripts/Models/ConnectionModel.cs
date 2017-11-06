using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ConnectionModel
{
    public readonly int Output;
    public readonly int Input;

    public ConnectionModel(int output, int input)
    {
        Output = output;
        Input = input;
    }
}
