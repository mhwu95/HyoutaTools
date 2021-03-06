﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace HyoutaTools {
	public static class Util {

		#region EndianUtils
		// baffling thing seen in Gust textures, 0xXY <-> 0xYX
		public static byte SwapEndian4Bits( this byte x ) {
			return (byte)( ( ( x & 0x0F ) << 4 ) | ( ( x & 0xF0 ) >> 4 ) );
		}
		public static Int16 SwapEndian( this Int16 x ) {
			return (Int16)SwapEndian( (UInt16)x );
		}
		public static UInt16 SwapEndian( this UInt16 x ) {
			return x = (UInt16)
					   ( ( x << 8 ) |
						( x >> 8 ) );
		}

		public static Int32 SwapEndian( this Int32 x ) {
			return (Int32)SwapEndian( (UInt32)x );
		}
		public static UInt32 SwapEndian( this UInt32 x ) {
			return x = ( x << 24 ) |
					  ( ( x << 8 ) & 0x00FF0000 ) |
					  ( ( x >> 8 ) & 0x0000FF00 ) |
					   ( x >> 24 );
		}

		public static Int64 SwapEndian( this Int64 x ) {
			return (Int64)SwapEndian( (UInt64)x );
		}
		public static UInt64 SwapEndian( this UInt64 x ) {
			return x = ( x << 56 ) |
						( ( x << 40 ) & 0x00FF000000000000 ) |
						( ( x << 24 ) & 0x0000FF0000000000 ) |
						( ( x << 8 ) & 0x000000FF00000000 ) |
						( ( x >> 8 ) & 0x00000000FF000000 ) |
						( ( x >> 24 ) & 0x0000000000FF0000 ) |
						( ( x >> 40 ) & 0x000000000000FF00 ) |
						 ( x >> 56 );
		}

		public enum Endianness { LittleEndian, BigEndian }
		// man why the hell can you not constraint generic parameters to integers?
		public static Int16 ToEndian( this Int16 x, Endianness endian ) {
			return (Int16)ToEndian( (UInt16)x, endian );
		}
		public static UInt16 ToEndian( this UInt16 x, Endianness endian ) {
			switch ( endian ) {
				case Endianness.LittleEndian: if ( BitConverter.IsLittleEndian ) { return x; } else { return x.SwapEndian(); }
				case Endianness.BigEndian: if ( BitConverter.IsLittleEndian ) { return x.SwapEndian(); } else { return x; }
				default: throw new Exception( "Invalid Endianness" );
			}
		}
		public static Int32 ToEndian( this Int32 x, Endianness endian ) {
			return (Int32)ToEndian( (UInt32)x, endian );
		}
		public static UInt32 ToEndian( this UInt32 x, Endianness endian ) {
			switch ( endian ) {
				case Endianness.LittleEndian: if ( BitConverter.IsLittleEndian ) { return x; } else { return x.SwapEndian(); }
				case Endianness.BigEndian: if ( BitConverter.IsLittleEndian ) { return x.SwapEndian(); } else { return x; }
				default: throw new Exception( "Invalid Endianness" );
			}
		}
		public static Int64 ToEndian( this Int64 x, Endianness endian ) {
			return (Int64)ToEndian( (UInt64)x, endian );
		}
		public static UInt64 ToEndian( this UInt64 x, Endianness endian ) {
			switch ( endian ) {
				case Endianness.LittleEndian: if ( BitConverter.IsLittleEndian ) { return x; } else { return x.SwapEndian(); }
				case Endianness.BigEndian: if ( BitConverter.IsLittleEndian ) { return x.SwapEndian(); } else { return x; }
				default: throw new Exception( "Invalid Endianness" );
			}
		}

		// honestly I'm not sure if it makes sense to have different To and From functions
		// since all cases I can think of result in the same thing, but better be safe than sorry,
		// and it also gives some information if we're reading in or writing out data
		public static Int16 FromEndian( this Int16 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		public static UInt16 FromEndian( this UInt16 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		public static Int32 FromEndian( this Int32 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		public static UInt32 FromEndian( this UInt32 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		public static Int64 FromEndian( this Int64 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		public static UInt64 FromEndian( this UInt64 x, Endianness endian ) {
			return ToEndian( x, endian );
		}
		#endregion

		#region HexUtils
		public static byte ParseDecOrHexToByte( string s ) {
			s = s.Trim();

			if ( s.StartsWith( "0x" ) ) {
				s = s.Substring( 2 );
				return Byte.Parse( s, System.Globalization.NumberStyles.HexNumber );
			} else {
				return Byte.Parse( s );
			}
		}

		public static uint ParseDecOrHex( string s ) {
			s = s.Trim();

			if ( s.StartsWith( "0x" ) ) {
				s = s.Substring( 2 );
				return UInt32.Parse( s, System.Globalization.NumberStyles.HexNumber );
			} else {
				return UInt32.Parse( s );
			}
		}

		// https://stackoverflow.com/a/9995303
		public static byte[] HexStringToByteArray( string hex ) {
			if ( hex.Length % 2 == 1 )
				throw new Exception( "The binary key cannot have an odd number of digits" );

			byte[] arr = new byte[hex.Length >> 1];

			for ( int i = 0; i < (hex.Length >> 1); ++i ) {
				arr[i] = (byte)( ( GetHexVal( hex[i << 1] ) << 4 ) + ( GetHexVal( hex[( i << 1 ) + 1] ) ) );
			}

			return arr;
		}

		public static int GetHexVal( char hex ) {
			int val = (int)hex;
			//For uppercase A-F letters:
			//return val - (val < 58 ? 48 : 55);
			//For lowercase a-f letters:
			//return val - (val < 58 ? 48 : 87);
			//Or the two combined, but a bit slower:
			return val - ( val < 58 ? 48 : ( val < 97 ? 55 : 87 ) );
		}
		#endregion

		#region NumberUtils
		public static uint ToUInt24( byte[] File, int Pointer ) {
			byte b1 = File[Pointer];
			byte b2 = File[Pointer + 1];
			byte b3 = File[Pointer + 2];

			return (uint)( b3 << 16 | b2 << 8 | b1 );
		}
		public static byte[] GetBytesForUInt24( uint Number ) {
			byte[] b = new byte[3];
			b[0] = (byte)( Number & 0xFF );
			b[1] = (byte)( ( Number >> 8 ) & 0xFF );
			b[2] = (byte)( ( Number >> 16 ) & 0xFF );
			return b;
		}

		/// <summary>
		/// converts a 32-bit int that's actually a byte representation of a float
		/// to an actual float for use in calculations or whatever
		/// </summary>
		public static float UIntToFloat( this uint integer ) {
			byte[] b = BitConverter.GetBytes( integer );
			float f = BitConverter.ToSingle( b, 0 );
			return f;
		}

		public static int Align( this int Number, int Alignment ) {
			return (int)Align( (uint)Number, (uint)Alignment );
		}
		public static uint Align( this uint Number, uint Alignment ) {
			uint diff = Number % Alignment;
			if ( diff == 0 ) {
				return Number;
			} else {
				return ( Number + ( Alignment - diff ) );
			}
		}
		public static long Align( this long Number, int Alignment ) {
			return (long)Align( (ulong)Number, (uint)Alignment );
		}
		public static ulong Align( this ulong Number, uint Alignment ) {
			ulong diff = Number % Alignment;
			if ( diff == 0 ) {
				return Number;
			} else {
				return ( Number + ( Alignment - diff ) );
			}
		}

		public static sbyte AsSigned( this byte Number ) {
			return (sbyte)Number;
		}
		public static short AsSigned( this ushort Number ) {
			return (short)Number;
		}
		public static int AsSigned( this uint Number ) {
			return (int)Number;
		}
		public static long AsSigned( this ulong Number ) {
			return (long)Number;
		}
		#endregion

		#region TextUtils
		private static Encoding _ShiftJISEncoding = null;
		public static Encoding ShiftJISEncoding { get { if ( _ShiftJISEncoding == null ) { _ShiftJISEncoding = Encoding.GetEncoding( 932 ); } return _ShiftJISEncoding; } }
		public static String GetTextShiftJis( byte[] File, int Pointer ) {
			if ( Pointer == -1 ) return null;

			try {
				int i = Pointer;
				while ( File[i] != 0x00 ) {
					i++;
				}
				String Text = ShiftJISEncoding.GetString( File, Pointer, i - Pointer );
				return Text;
			} catch ( Exception ) {
				return null;
			}
		}
		public static String GetTextAscii( byte[] File, int Pointer ) {
			if ( Pointer == -1 ) return null;

			try {
				int i = Pointer;
				while ( File[i] != 0x00 ) {
					i++;
				}
				String Text = Encoding.ASCII.GetString( File, Pointer, i - Pointer );
				return Text;
			} catch ( Exception ) {
				return null;
			}
		}
		public static String GetTextUnicode( byte[] File, int Pointer, int MaxByteLength ) {
			StringBuilder sb = new StringBuilder();
			for ( int i = 0; i < MaxByteLength; i += 2 ) {
				ushort ch = BitConverter.ToUInt16( File, Pointer + i );
				if ( ch == 0 || ch == 0xFFFF ) { break; }
				sb.Append( (char)ch );
			}
			return sb.ToString();
		}
		public static String GetTextUTF8( byte[] File, int Pointer ) {
			int tmp;
			return GetTextUTF8( File, Pointer, out tmp );
		}
		public static String GetTextUTF8( byte[] File, int Pointer, out int NullLocation ) {
			if ( Pointer == -1 ) { NullLocation = -1; return null; }

			try {
				int i = Pointer;
				while ( File[i] != 0x00 ) {
					i++;
				}
				String Text = Encoding.UTF8.GetString( File, Pointer, i - Pointer );
				NullLocation = i;
				return Text;
			} catch ( Exception ) {
				NullLocation = -1;
				return null;
			}
		}
		public static String TrimNull( this String s ) {
			int n = s.IndexOf( '\0', 0 );
			if ( n >= 0 ) {
				return s.Substring( 0, n );
			}
			return s;
		}
		public static byte[] StringToBytesShiftJis( String s ) {
			//byte[] bytes = ShiftJISEncoding.GetBytes(s);
			//return bytes.TakeWhile(subject => subject != 0x00).ToArray();
			return ShiftJISEncoding.GetBytes( s );
		}
		public static byte[] StringToBytesUTF16( String s ) {
			return Encoding.Unicode.GetBytes( s );
		}

		public static string XmlEscape( string s ) {
			s = s.Replace( "&", "&amp;" );
			s = s.Replace( "\"", "&quot;" );
			s = s.Replace( "'", "&apos;" );
			s = s.Replace( "<", "&lt;" );
			s = s.Replace( ">", "&gt;" );
			return s;
		}

		public static string Truncate( this string value, int maxLength ) {
			if ( string.IsNullOrEmpty( value ) ) return value;
			return value.Length <= maxLength ? value : value.Substring( 0, maxLength );
		}

		// from http://stackoverflow.com/a/25471811
		public static string UnEscape( this string s ) {
			StringBuilder sb = new StringBuilder();
			Regex r = new Regex( "\\\\[abfnrtv?\"'\\\\]|\\\\[0-3]?[0-7]{1,2}|\\\\u[0-9a-fA-F]{4}|\\\\U[0-9a-fA-F]{8}|." );
			MatchCollection mc = r.Matches( s, 0 );

			foreach ( Match m in mc ) {
				if ( m.Length == 1 ) {
					sb.Append( m.Value );
				} else {
					if ( m.Value[1] >= '0' && m.Value[1] <= '7' ) {
						int i = Convert.ToInt32( m.Value.Substring( 1 ), 8 );
						sb.Append( (char)i );
					} else if ( m.Value[1] == 'u' ) {
						int i = Convert.ToInt32( m.Value.Substring( 2 ), 16 );
						sb.Append( (char)i );
					} else if ( m.Value[1] == 'U' ) {
						int i = Convert.ToInt32( m.Value.Substring( 2 ), 16 );
						sb.Append( char.ConvertFromUtf32( i ) );
					} else {
						switch ( m.Value[1] ) {
							case 'a':
								sb.Append( '\a' );
								break;
							case 'b':
								sb.Append( '\b' );
								break;
							case 'f':
								sb.Append( '\f' );
								break;
							case 'n':
								sb.Append( '\n' );
								break;
							case 'r':
								sb.Append( '\r' );
								break;
							case 't':
								sb.Append( '\t' );
								break;
							case 'v':
								sb.Append( '\v' );
								break;
							default:
								sb.Append( m.Value[1] );
								break;
						}
					}
				}
			}

			return sb.ToString();
		}

		// https://stackoverflow.com/questions/188892/glob-pattern-matching-in-net
		/// <summary>
		/// Compares the string against a given pattern.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <param name="pattern">The pattern to match, where "*" means any sequence of characters, and "?" means any single character.</param>
		/// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
		public static bool Like( this string str, string pattern ) {
			return new Regex(
				"^" + Regex.Escape( pattern ).Replace( @"\*", ".*" ).Replace( @"\?", "." ) + "$",
				RegexOptions.IgnoreCase | RegexOptions.Singleline
			).IsMatch( str );
		}
		#endregion

		#region TimeUtils
		public static DateTime UnixTimeToDateTime( ulong unixTime ) {
			return new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ).AddSeconds( unixTime ).ToLocalTime();
		}
		public static ulong DateTimeToUnixTime( DateTime time ) {
			return (ulong)( time - new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ).ToLocalTime() ).TotalSeconds;
		}
		public static DateTime PS3TimeToDateTime( ulong PS3Time ) {
			return new DateTime( 1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ).AddMilliseconds( PS3Time / 1000 ).ToLocalTime();
		}
		#endregion

		#region ArrayUtils
		public static void CopyByteArrayPart( IList<byte> from, int locationFrom, IList<byte> to, int locationTo, int count ) {
			for ( int i = 0; i < count; i++ ) {
				to[locationTo + i] = from[locationFrom + i];
			}
		}

		public static void FillNull( IList<byte> Array, int Location, int Count ) {
			for ( int i = 0; i < Count; ++i ) {
				Array[Location + i] = 0x00;
			}
		}

		public static bool IsByteArrayPartEqual( IList<byte> Array1, int Location1, IList<byte> Array2, int Location2, int count ) {
			for ( int i = 0; i < count; ++i ) {
				if ( Array1[i + Location1] != Array2[i + Location2] ) {
					return false;
				}
			}
			return true;
		}
		#endregion

		#region StreamUtils
		public static void CopyStream( System.IO.Stream input, System.IO.Stream output, long count ) {
			byte[] buffer = new byte[4096];
			int read;

			long bytesLeft = count;
			while ( ( read = input.Read( buffer, 0, (int)Math.Min( buffer.LongLength, bytesLeft ) ) ) > 0 ) {
				output.Write( buffer, 0, read );
				bytesLeft -= read;
				if ( bytesLeft <= 0 ) return;
			}
		}

		public static bool IsIdentical( this Stream str, Stream other, long count ) {
			for ( long i = 0; i < count; ++i ) {
				if ( str.ReadByte() != other.ReadByte() ) {
					return false;
				}
			}
			return true;
		}

		public static ulong ReadUInt64( this Stream s ) {
			ulong b1 = (ulong)s.ReadByte();
			ulong b2 = (ulong)s.ReadByte();
			ulong b3 = (ulong)s.ReadByte();
			ulong b4 = (ulong)s.ReadByte();
			ulong b5 = (ulong)s.ReadByte();
			ulong b6 = (ulong)s.ReadByte();
			ulong b7 = (ulong)s.ReadByte();
			ulong b8 = (ulong)s.ReadByte();

			return (ulong)( b8 << 56 | b7 << 48 | b6 << 40 | b5 << 32 | b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static ulong PeekUInt64( this Stream s ) {
			long pos = s.Position;
			ulong retval = s.ReadUInt64();
			s.Position = pos;
			return retval;
		}
		public static void WriteUInt64( this Stream s, ulong num ) {
			s.Write( BitConverter.GetBytes( num ), 0, 8 );
		}
		public static ulong ReadUInt56( this Stream s ) {
			ulong b1 = (ulong)s.ReadByte();
			ulong b2 = (ulong)s.ReadByte();
			ulong b3 = (ulong)s.ReadByte();
			ulong b4 = (ulong)s.ReadByte();
			ulong b5 = (ulong)s.ReadByte();
			ulong b6 = (ulong)s.ReadByte();
			ulong b7 = (ulong)s.ReadByte();

			return (ulong)( b7 << 48 | b6 << 40 | b5 << 32 | b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static ulong PeekUInt56( this Stream s ) {
			long pos = s.Position;
			ulong retval = s.ReadUInt56();
			s.Position = pos;
			return retval;
		}
		public static ulong ReadUInt48( this Stream s ) {
			ulong b1 = (ulong)s.ReadByte();
			ulong b2 = (ulong)s.ReadByte();
			ulong b3 = (ulong)s.ReadByte();
			ulong b4 = (ulong)s.ReadByte();
			ulong b5 = (ulong)s.ReadByte();
			ulong b6 = (ulong)s.ReadByte();

			return (ulong)( b6 << 40 | b5 << 32 | b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static ulong PeekUInt48( this Stream s ) {
			long pos = s.Position;
			ulong retval = s.ReadUInt48();
			s.Position = pos;
			return retval;
		}
		public static ulong ReadUInt40( this Stream s ) {
			ulong b1 = (ulong)s.ReadByte();
			ulong b2 = (ulong)s.ReadByte();
			ulong b3 = (ulong)s.ReadByte();
			ulong b4 = (ulong)s.ReadByte();
			ulong b5 = (ulong)s.ReadByte();

			return (ulong)( b5 << 32 | b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static ulong PeekUInt40( this Stream s ) {
			long pos = s.Position;
			ulong retval = s.ReadUInt40();
			s.Position = pos;
			return retval;
		}
		public static uint ReadUInt32( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();
			int b3 = s.ReadByte();
			int b4 = s.ReadByte();

			return (uint)( b4 << 24 | b3 << 16 | b2 << 8 | b1 );
		}
		public static uint PeekUInt32( this Stream s ) {
			long pos = s.Position;
			uint retval = s.ReadUInt32();
			s.Position = pos;
			return retval;
		}
		public static void WriteUInt32( this Stream s, uint num ) {
			s.Write( BitConverter.GetBytes( num ), 0, 4 );
		}
		public static uint ReadUInt24( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();
			int b3 = s.ReadByte();

			return (uint)( b3 << 16 | b2 << 8 | b1 );
		}
		public static uint PeekUInt24( this Stream s ) {
			long pos = s.Position;
			uint retval = s.ReadUInt24();
			s.Position = pos;
			return retval;
		}
		public static ushort ReadUInt16( this Stream s ) {
			int b1 = s.ReadByte();
			int b2 = s.ReadByte();

			return (ushort)( b2 << 8 | b1 );
		}
		public static ushort PeekUInt16( this Stream s ) {
			long pos = s.Position;
			ushort retval = s.ReadUInt16();
			s.Position = pos;
			return retval;
		}
		public static byte ReadUInt8( this Stream s ) {
			return Convert.ToByte( s.ReadByte() );
		}
		public static byte PeekByte( this Stream s ) {
			long pos = s.Position;
			int retval = s.ReadByte();
			s.Position = pos;
			return Convert.ToByte( retval );
		}
		public static void DiscardBytes( this Stream s, uint count ) {
			s.Position = s.Position + count;
		}
		public static void WriteUInt16( this Stream s, ushort num ) {
			s.Write( BitConverter.GetBytes( num ), 0, 2 );
		}

		public static byte[] ReadUInt8Array( this Stream s, long count ) {
			byte[] data = new byte[count];
			for ( long i = 0; i < count; ++i ) {
				data[i] = s.ReadUInt8();
			}
			return data;
		}

		public static uint[] ReadUInt32Array( this Stream s, long count, Endianness endianness = Endianness.LittleEndian ) {
			uint[] data = new uint[count];
			for ( long i = 0; i < count; ++i ) {
				data[i] = s.ReadUInt32().FromEndian( endianness );
			}
			return data;
		}

		public static void ReadAlign( this Stream s, long alignment ) {
			while ( s.Position % alignment != 0 ) {
				s.DiscardBytes( 1 );
			}
		}
		public static void WriteAlign( this Stream s, long alignment, byte paddingByte = 0 ) {
			while ( s.Position % alignment != 0 ) {
				s.WriteByte( paddingByte );
			}
		}

		public static string ReadAsciiNulltermFromLocationAndReset( this Stream s, long location ) {
			long pos = s.Position;
			s.Position = location;
			string str = s.ReadAsciiNullterm();
			s.Position = pos;
			return str;
		}
		public static string ReadAsciiNullterm( this Stream s ) {
			StringBuilder sb = new StringBuilder();
			int b = s.ReadByte();
			while ( b != 0 && b != -1 ) {
				sb.Append( (char)( b ) );
				b = s.ReadByte();
			}
			return sb.ToString();
		}
		public static string ReadAscii( this Stream s, int count ) {
			StringBuilder sb = new StringBuilder( count );
			int b;
			for ( int i = 0; i < count; ++i ) {
				b = s.ReadByte();
				sb.Append( (char)( b ) );
			}
			return sb.ToString();
		}
		public static void WriteAscii( this Stream s, string str, int count = 0, bool trim = false ) {
			WriteString( s, Encoding.ASCII, str, count, trim );
		}
        public static void WriteString( this Stream s, Encoding encoding, string str, int count = 0, bool trim = false ) {
			byte[] chars = encoding.GetBytes( str );
			if ( !trim && count > 0 && count < chars.Length ) {
				throw new Exception( "String won't fit in provided space!" );
			}

			int i;
			for ( i = 0; i < chars.Length; ++i ) {
				s.WriteByte( chars[i] );
			}
			for ( ; i < count; ++i ) {
				s.WriteByte( 0 );
			}
		}
		public static string ReadUTF8Nullterm( this Stream s ) {
			List<byte> data = new List<byte>();
			int b = s.ReadByte();
			while ( b != 0 && b != -1 ) {
				data.Add( (byte)( b ) );
				b = s.ReadByte();
			}
			return Encoding.UTF8.GetString( data.ToArray() );
		}
		public static void WriteUTF8( this Stream s, string str, int count = 0, bool trim = false ) {
			WriteString( s, Encoding.UTF8, str, count, trim );
		}
		public static void WriteUTF8Nullterm( this Stream s, string str ) {
			WriteUTF8( s, str, 0, false );
			s.WriteByte( 0 );
		}
		public static string ReadUTF16Nullterm( this Stream s ) {
			StringBuilder sb = new StringBuilder();
			byte[] b = new byte[2];
			int b0 = s.ReadByte();
			int b1 = s.ReadByte();
			while ( !( b0 == 0 && b1 == 0 ) && b1 != -1 ) {
				b[0] = (byte)b0; b[1] = (byte)b1;
				sb.Append( Encoding.Unicode.GetString( b, 0, 2 ) );
				b0 = s.ReadByte(); b1 = s.ReadByte();
			}
			return sb.ToString();
		}
		public static string ReadShiftJisNullterm( this Stream s ) {
			StringBuilder sb = new StringBuilder();
			byte[] buffer = new byte[2];

			int b = s.ReadByte();
			while ( b != 0 && b != -1 ) {
				if ( ( b >= 0 && b <= 0x80 ) || ( b >= 0xA0 && b <= 0xDF ) ) {
					// is a single byte
					buffer[0] = (byte)b;
					sb.Append( ShiftJISEncoding.GetString( buffer, 0, 1 ) );
				} else {
					// is two bytes
					buffer[0] = (byte)b;
					buffer[1] = (byte)s.ReadByte();
					sb.Append( ShiftJISEncoding.GetString( buffer ) );
				}
				b = s.ReadByte();
			}
			return sb.ToString();
		}

		public static void Write( this Stream s, byte[] data ) {
			s.Write( data, 0, data.Length );
		}
		#endregion

		public static string GuessFileExtension( Stream s ) {
			uint magic32 = s.PeekUInt32();
			uint magic24 = s.PeekUInt24();
			uint magic16 = s.PeekUInt16();

			switch ( magic32 ) {
				case 0x46464952:
					return ".wav";
				case 0x474E5089:
					return ".png";
				case 0x5367674F:
					return ".ogg";
			}
			switch ( magic16 ) {
				case 0x4D42:
					return ".bmp";
			}

			return "";
		}


		public static void Assert( bool cond ) {
			if ( !cond ) {
				throw new Exception( "Assert Failed!" );
			}
		}
	}
}
