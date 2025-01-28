﻿//
// MimeEntityConstructorArgs.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2025 .NET Foundation and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System.Collections.Generic;

namespace MimeKit {
	/// <summary>
	/// MIME entity constructor arguments.
	/// </summary>
	/// <remarks>
	/// MIME entity constructor arguments.
	/// </remarks>
	public sealed class MimeEntityConstructorArgs
	{
		internal readonly ParserOptions ParserOptions;
		internal readonly IEnumerable<Header> Headers;
		internal readonly ContentType ContentType;
		internal readonly bool IsTopLevel;

		internal MimeEntityConstructorArgs (ParserOptions options, ContentType ctype, IEnumerable<Header> headers, bool toplevel)
		{
			ParserOptions = options;
			IsTopLevel = toplevel;
			ContentType = ctype;
			Headers = headers;
		}
	}
}
