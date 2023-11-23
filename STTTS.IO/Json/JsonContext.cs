﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace STTTS.IO.Json;

[JsonSerializable(typeof(string[]), TypeInfoPropertyName = "StringArray")]
[JsonSerializable(typeof(Dictionary<string, List<string>>), TypeInfoPropertyName = "StringListDictionary")]
public partial class JsonContext : JsonSerializerContext { }
