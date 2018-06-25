using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SLApp
{
    public interface ISpeech
    {
        Task<string[]> SayWords();

    }

}
