namespace Utilities
{
    using System;
    using System.IO;
    using System.Reflection;

    public static partial class UnityExtensionUtility
    {
        #region string path methods

        /// <summary>
        /// Returns true if the string represents a file system path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPath(this string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            return Path.IsPathRooted(path); // For now we'll require fully "rooted" paths. We can do more later.
        }

        public static string PathToUrl(this string path)
        {
            Debug.Assert(path.IsPath() && !path.IsUrl());
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WSA || WINDOWS_UWP)
            return "file:///" + path.Replace(@"\", @"/");
#else
        return "file://" + path;
#endif
        }

        public static string EnsurePath(this string maybePath)
        {
            if (maybePath.IsFileUrl())
                return maybePath.UrlToPath();
            Debug.Assert(maybePath.IsPath(), "we're only dealing with paths and file urls, not web urls or arbitrary strings");
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WSA || WINDOWS_UWP)
            return maybePath.Replace(@"/", @"\"); // This isn't necessarily correct for paths that embed the wrong slash type.
#else
        return maybePath;
#endif
        }

        #endregion

        #region string url methods

        public enum UrlType
        {
            http = 1,
            https = 2,
            ftp = 4,
            file = 8,
            web = (http | https | ftp),
            any = (http | https | ftp | file),
        }

        /// <summary>
        /// Returns true if the string represents a url that Unity will be happy with. For now that means http://, https://, ftp:// and file://
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(this string url, UrlType validType = UrlType.any)
        {
            // Note: When using file protocol on Windows and Windows Store Apps for accessing local files, you have to specify file:/// (with three slashes).
            if (string.IsNullOrEmpty(url))
                return false;

            // In general URIs as defined by RFC 3986(see Section 2: Characters) may contain any of the following characters:
            //
            //    ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 -._~:/?#[]@!$&'()*+,;=`.
            //
            // Any other character needs to be encoded with the percent-encoding(% hh).Each part of the URI has further
            // restrictions about what characters need to be represented by an percent - encoded word.
            // That means "\" is allowed in a URL as %-encoded only.
            //
            // I may want to add a more stringent test for legal character usage in the url.

            var lower = url.ToLowerInvariant();
            return lower.StartsWith("http://") || lower.StartsWith("https://") || lower.StartsWith("ftp://") || lower.StartsWith("file://");
        }

        public static bool IsFileUrl(this string url)
        {
            return url.IsUrl(UrlType.file);
        }

        public static bool IsWebUrl(this string url)
        {
            return url.IsUrl(UrlType.web);
        }

        public static string UrlToPath(this string url)
        {
            Debug.Assert(url.IsUrl(UrlType.file) && !url.IsPath(), "can only convert file:// urls to paths");
            int discard = (url.ToLowerInvariant().StartsWith("file:///") ? 8 : 7);
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WSA || WINDOWS_UWP)
            return url.Substring(discard).Replace(@"/", @"\");
#else
        return url.Substring(discard);
#endif
        }

        public static string EnsureUrl(this string maybeUrl)
        {
            if (maybeUrl.IsPath())
                return maybeUrl.PathToUrl();
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_XBOXONE || UNITY_WSA || WINDOWS_UWP)
            maybeUrl = maybeUrl.Replace(@"\", @"/"); // Is this doing any favors? Isn't this the wrong place? See the RFC 3986 discussion.
#endif
            Debug.Assert(maybeUrl.IsUrl(), "we're only dealing with paths and urls, not arbitrary strings");
            return maybeUrl;
        }

        #endregion
    }


    

    /// <summary>
    /// Some utilities related to file paths.
    /// </summary>
    public static class PathUtility
    {
        public static char[] PathSeparators = new char[] { '\\', '/' };

        /// <summary>
        /// Gets the just the file name from a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getFileName(string path) // Maybe rename to indicate it can be called with paths or urls?
        {
#if false
            // Unity already provides a cross-platform implementation for this one if "path" is actually a path and not a url.
            // I suspect most arguments to this method will actually be urls and not file paths?
            return Path.GetFileName(path); // This still won't strip off any url arguments if the path is actually a url.
#elif false
            // Definitely unneeded allocations here.
            string[] splitted = path.Split(PathSeparators);
            return splitted[splitted.Length - 1];
#else
            int lastSlash = path.LastIndexOfAny(PathSeparators);
            if (lastSlash >= 0)
            {
                return path.Substring(lastSlash + 1);
            }
            return path;
#endif
        }

        /// <summary>
        /// Gets the folder path from a model or texture path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string getFolderPath(string path) // Ditto the rename comment?
        {
#if false
            // Of course if we want to specifically handle urls we should probably differentiate between paths and urls.
            // The same utilities will usually operate on both just fine if all separators are supported.
            return Path.GetDirectoryName(path); // This is converting all forward slashes to back slashes on Windows.
#else
            int lastSlash = path.LastIndexOfAny(PathSeparators);
            if (lastSlash >= 0)
            {
                return path.Substring(0, lastSlash + 1); // We're leaving a trailing separator. Other routines may not do this.
            }
            return path;
#endif
        }

        /// <summary>
        /// Checks if the path is valid for models by making sure its got enough characters and .dae on the end
        /// </summary>
        /// <param name="importPath"></param>
        /// <returns></returns>
        public static bool isValidModelPath(string importPath)
        {
#if true
            if (string.IsNullOrEmpty(importPath))
                return false;

            // Just test the ends. Handle uppercase paths too.
            var ignCase = System.StringComparison.InvariantCultureIgnoreCase;
            // So sad, just collada. :(  We'll want to someday add support for the compressed .zae format too.
            return (importPath.EndsWith(".dae", ignCase));
#else
            if (importPath == null || importPath.Length < 4)
                return false;
            if (!importPath.Substring(importPath.Length - 4).Equals(".dae"))
                return false;
            return true;
#endif
        }

        /// <summary>
        /// Checks if the path is valid for the fast texture importer by making sure its got enough characters and the correct file type on the end
        /// Im using the c# bitmap class for importing which supports BMP, GIF, EXIF, JPG, PNG and TIFF.
        /// </summary>
        /// <param name="importPath"></param>
        /// <returns></returns>
        public static bool isValidFastTexturePath(string importPath)
        {
#if true
            if (string.IsNullOrEmpty(importPath))
                return false;

            // Just test the ends. Handle uppercase paths too.
            var ignCase = System.StringComparison.InvariantCultureIgnoreCase;
            return (importPath.EndsWith(".bmp", ignCase) ||
                importPath.EndsWith(".jpg", ignCase) ||
                importPath.EndsWith(".png", ignCase) ||
                importPath.EndsWith(".gif", ignCase) ||
                importPath.EndsWith(".exif", ignCase) ||
                importPath.EndsWith(".tiff", ignCase));
#else
            // I don't find a call to this method. It'll be a problem when we start seeing hdr and exr texture maps.
            if (importPath == null || importPath.Length < 4)
                return false;
            if (importPath.Substring(importPath.Length - 4).Equals(".bmp") ||
                importPath.Substring(importPath.Length - 4).Equals(".jpg") ||
                importPath.Substring(importPath.Length - 4).Equals(".png") ||
                importPath.Substring(importPath.Length - 4).Equals(".gif") ||
                importPath.Substring(importPath.Length - 5).Equals(".exif") ||
                importPath.Substring(importPath.Length - 5).Equals(".tiff"))
                return true;
            else
                return false;
#endif
        }

        /// <summary>
        /// Converts a url (file://// or http:///) to a dos file path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertURLToPath(string path /* url? */)
        {
#if true
            // Use the portable string extension method. This expects file:// prefixed urls. How is it supposed to work for http?
            return path.EnsurePath();
#else
            if (path.Substring(0, 4) == "file" || path.Substring(0, 4) == "http")
            {
                return path.Substring(8).Replace(@"/", @"\");
            }
            else
                return path;
#endif
        }

        /// <summary>
        /// Converts a dos file path to a url (file:///)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertPathToURL(string path)
        {
#if true
            // Use the portable string extension method.
            return path.EnsureUrl();
#else
            if (path.Substring(0, 4) != "file" && path.Substring(0, 4) != "http")
            {
                return "file:///" + path.Replace(@"\", @"/");
            }
            else
                return path;
#endif
        }

        /// <summary>
        /// This returns the root directory, which in the editor is the parent of "Assets/"
        /// and in the player is the read-only content directory.
        /// </summary>
        /// <param name="appending"></param>
        /// <returns></returns>
        public static string GetRootDataPath(string appending = null)
        {
            // Unity guarantees that in the editor Application.dataPath will get the assets folder.
            // https://docs.unity3d.com/ScriptReference/Application-dataPath.html
            string rootPath = (UnityEngine.Application.isEditor
                ? Path.GetDirectoryName(UnityEngine.Application.dataPath)
                : Path.GetFullPath("."));

            // Personally, I have very little faith in the current working directory,
            // but Unity has added runtime checks to ensure it remains consistent, so...
            UnityEngine.Assertions.Assert.AreEqual(Path.GetFullPath(rootPath), Directory.GetCurrentDirectory());

            // We need a normalized Path.Combine operator. On Windows, GetFullPath returns a path with backslashes.
            // But the System.IO.Path routines return forward slashes. It's weird! I want a clean, consistent result.
            // Another argument for our own Path ADT and supporting operations.
            if (!string.IsNullOrEmpty(appending))
                rootPath = Path.GetFullPath(Path.Combine(rootPath, appending));

            return rootPath;
        }

#if UNITY_EDITOR
        /// <summary>
        /// This method returns the root of the project, the parent of the Assets folder.
        /// As such it isn't appropriate to call it from a built player.
        /// </summary>
        /// <param name="appending"></param>
        /// <returns></returns>
        public static string GetProjectRootPath(string appending = null)
        {
            // but then again...
            Debug.Assert(UnityEngine.Application.isEditor);
            return GetRootDataPath(appending);
        }
#endif

#if NEVER_GONNA_DO
        public static void BullHockey(string writingToPath)
        {
            // Mark expressed a concern that we might write into our git repo folder.
            // In an editor build we could assert that a user path won't do that.
            string checkResult = Path.GetFullPath(writingToPath);
            string checkRoot = Path.GetFullPath(Utils.PathUtility.GetProjectRootPath());
            Debug.Assert(!checkResult.StartsWith(checkRoot),
                "We shouldn't be writing asset bundles into our own repository space");
        }
#endif
    }
}
