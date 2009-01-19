COMPILATION
===========

The compilation of SQLite.NET is not straightforward. Because SQLite requires the sqlite_exec callback
function to be called using the Cdecl convention, some hacking is required. C# itself does not support
Cdecl convention for delegates, however MSIL does. This means the compiled Dll must be decompiled to IL,
the IL modified, and then recompiled to a Dll. See the file "Cdecl Delegates.txt" for in depth details.

This hacking is handled by the post build event for SQLiteClient, a batch script and a JScript script.


LICENSING
=========

SQLite.NET is licensed as GPL. This means if you use it in your application, your app must also be GPL.


DOCUMENTATION
=============

Docs are available in the SQLiteClient/doc/doc/ directory. Open the SQLite.NET.chm file.