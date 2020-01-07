class BrunoHash 
{
        /// <summary>
		/// The fastest hash algorithm.
		/// This algorithm is designed not to burden the CPU in all those cases where frequent hashing operations are required: cryptography, digital signatures, checksum affixing, cryptocurrency, etc.
		/// </summary>
		/// <param name="data">The data on which to compute the hash</param>
		/// <returns>A 32-byte hash (256 bit)</returns>
		public static byte[] GetBrunoHash(byte[] data)
		{
			int[] seeds = new int[8];
            byte[] br1 = new byte[8];
			byte[] br2 = new byte[8];
			byte[] br3 = new byte[8];
			byte[] br4 = new byte[8];
			const int st1 = BitConverter.IsLittleEndian ? 4 : 0;
			const int st2 = BitConverter.IsLittleEndian ? 0 : 4;
            ulong p1 = 0, p2 = 0, p3 = 0, p4 = 0;

			if (data == null)
            {
				data = new byte[0];
            }
			byte[] newData = new byte[data.Length + 32];
			data.CopyTo(newData, 0);
			
			for (int i = 0; i < data.Length; i = i + 32)
			{
				p1 = p1 ^ BitConverter.ToUInt64(newData, i);
				p2 = p2 ^ BitConverter.ToUInt64(newData, i + 8);
				p3 = p3 ^ BitConverter.ToUInt64(newData, i + 16);
				p4 = p4 ^ BitConverter.ToUInt64(newData, i + 24);
			}

			seeds[0] = BitConverter.ToInt32(BitConverter.GetBytes(p1), st1);
			seeds[1] = BitConverter.ToInt32(BitConverter.GetBytes(p1), st2);
			seeds[2] = BitConverter.ToInt32(BitConverter.GetBytes(p2), st1);
			seeds[3] = BitConverter.ToInt32(BitConverter.GetBytes(p2), st2);
			seeds[4] = BitConverter.ToInt32(BitConverter.GetBytes(p3), st1);
			seeds[5] = BitConverter.ToInt32(BitConverter.GetBytes(p3), st2);
			seeds[6] = BitConverter.ToInt32(BitConverter.GetBytes(p4), st1);
			seeds[7] = BitConverter.ToInt32(BitConverter.GetBytes(p4), st2);

			foreach (var seed in seeds)
			{
				var rnd = new Random(seed);
				rnd.NextBytes(br1);
				rnd.NextBytes(br2);
				rnd.NextBytes(br3);
				rnd.NextBytes(br4);
				p1 = p1 ^ BitConverter.ToUInt64(br1, 0);
				p2 = p2 ^ BitConverter.ToUInt64(br2, 0);
				p3 = p3 ^ BitConverter.ToUInt64(br3, 0);
				p4 = p4 ^ BitConverter.ToUInt64(br4, 0);
			}

			return BitConverter
                .GetBytes(p1)
                .Concat(BitConverter.GetBytes(p2))
                .Concat(BitConverter.GetBytes(p3))
                .Concat(BitConverter.GetBytes(p4))
                .ToArray();
		}
}
