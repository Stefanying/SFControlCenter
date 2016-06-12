using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace ControlCenter
{
    public class UrlHelper
    {
        const int MAX_URI_LENGTH = 512;
        string _scriptName = string.Empty;
        CommandResult _parseResult = CommandResult.Success;
        NameValueCollection _parameters = new NameValueCollection();
        char[] _uriInvalidChar = new char[] { '\\' };
        char[] _pathInvalidChar = new char[] { '\\', ':', '*', '?', '\"', '<', '>', '|' };
        public Uri _uri = null;

        public string ScriptName
        {
            get { return _scriptName; }
        }

        public NameValueCollection Parameters
        {
            get { return _parameters; }
        }

        public CommandResult ParseResult
        {
            get { return _parseResult; }
        }

        public UrlHelper(Uri originalUri)
        {
            _uri = originalUri;

            if (IsUriLengthError())
            {
                return;
            }

            if (CheckPathAndQuery())
            {
                ParsePathAndQuery();
            }
        }

        private bool IsUriLengthError()
        {
            if (_uri == null || _uri.ToString().Length > MAX_URI_LENGTH)
            {
                _parseResult = CommandResult.UrlTooLong;
                return true;
            }
            return false;
        }

        private bool CheckPathAndQuery()
        {
            string pathAndQuery = _uri.PathAndQuery.Substring(1);

            if (IsUrlInvalidChar(pathAndQuery))
            {
                return false;
            }

            if (pathAndQuery.IndexOfAny(_uriInvalidChar) >= 0)
            {
                _parseResult = CommandResult.UrlInvalidChar;
                return false;
            }
            else if (pathAndQuery.Length == 0)
            {
                _parseResult = CommandResult.NoExistsMethod;
                return false;
            }

            string[] splitPathAndQuery = new string[] { };
            if (IsFileNameInvalidChar(pathAndQuery, splitPathAndQuery))
            {
                return false;
            }

            return true;

        }

        private bool IsFileNameInvalidChar(string pathAndQuery, string[] splitPathAndQuery)
        {
            splitPathAndQuery = pathAndQuery.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitPathAndQuery[0].IndexOfAny(_pathInvalidChar) >= 0)
            {
                _parseResult = CommandResult.FileNameInvalidChar;
                return true;
            }
            return false;
        }

        private bool IsUrlInvalidChar(string pathAndQuery)
        {
            if (pathAndQuery.IndexOfAny(_uriInvalidChar) >= 0)
            {
                _parseResult = CommandResult.UrlInvalidChar;
                return true;
            }
            return false;
        }

        private void ParsePathAndQuery()
        {
            string[] splitPathAndQuery = _uri.PathAndQuery.Substring(1).Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            SetScriptNameAndParameters(splitPathAndQuery);
        }

        private void SetScriptNameAndParameters(string[] splitPathAndQuery)
        {
            _scriptName = splitPathAndQuery[0];

            if (splitPathAndQuery.Length > 1)
            {
                _parameters = HttpUtility.ParseQueryString(splitPathAndQuery[1], Encoding.UTF8);
            }
        }

    }
}
