using System;
using System.IO;

namespace prg2xex
{
    class Program
    {
        private static void PrintUsageAndExit()
        {
            Console.WriteLine("prg2xex Converts Commodore .prg file to Atari .xex file.\n(C) Pawel Kadluczka, 2011\n");
            Console.WriteLine("Usage: prg2xex <inputfile> <outputfile>\ninputfile - Commodore 64 .prg file\noutputfile - Atari .xex file\n");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsageAndExit();
            }

            Convert(args[0], args[1]);
        }

        private static void Convert(string inputFile, string outputFile)
        {
            const int MaxPrgFileSize = 0xffff;
            byte[] prg = new byte[MaxPrgFileSize + 1];
            int prgSize = -1;
            using (var input = new FileStream(inputFile, FileMode.Open))
            {
                Console.WriteLine("Reading {0}...", inputFile);
                prgSize = input.Read(prg, 0, MaxPrgFileSize + 1) - 2;
                if (prgSize <= 0)
                {
                    throw new InvalidOperationException("Invalid .prg file. At least one instruction expected");
                }
            }

            ushort startAddress = (ushort)(prg[0] + (prg[1] << 8));

            ushort endAddress;
            try
            {
                checked
                {
                    endAddress = (ushort)(startAddress + prgSize);
                }
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException("Invalid .prg file. Some instructions located beyond 0xffff address.");
            }

            using (var output = new FileStream(outputFile, FileMode.Create))
            {
                Console.WriteLine("Writing {0}...", outputFile);
                output.WriteByte(0xff);
                output.WriteByte(0xff);
                output.WriteByte(0xe0);
                output.WriteByte(0x02);
                output.WriteByte(0xe1);
                output.WriteByte(0x02);
                output.WriteByte((byte)(startAddress & 0xff));
                output.WriteByte((byte)((startAddress >> 8) & 0xff));
                output.WriteByte((byte)(startAddress & 0xff));
                output.WriteByte((byte)((startAddress >> 8) & 0xff));
                output.WriteByte((byte)(endAddress & 0xff));
                output.WriteByte((byte)((endAddress >> 8) & 0xff));
                output.Write(prg, 2, prgSize);
            }

            Console.WriteLine("Conversion completed.");
        }
    }
}