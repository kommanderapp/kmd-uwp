using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer.Models
{
    public class ExplorerNavigationHistory
    {
        public ExplorerNavigationHistory()
        {
            _oldList = new List<IStorageFolder>();
            _newList = new List<IStorageFolder>();
        }

        public bool CanGoBackward => _oldList.Count > 0;
        public bool CanGoForward => _newList.Count > 0;
        public IStorageFolder Current => _current;

        public IStorageFolder NavigateBackward()
        {
            Debug.Assert(CanGoBackward);

            if (_oldList.Count == 0)
                return null;

            var old = _oldList[_oldList.Count - 1];
            _oldList.RemoveAt(_oldList.Count - 1);
            _newList.Add(_current);
            _current = old;

            return _current;
        }

        public IStorageFolder NavigateForward()
        {
            Debug.Assert(CanGoForward);

            if (_newList.Count == 0)
                return null;

            var old = _newList[_newList.Count - 1];
            _newList.RemoveAt(_newList.Count - 1);
            _oldList.Add(_current);
            _current = old;

            return _current;
        }

        public void SetCurrent(IStorageFolder folder)
        {
            Debug.Assert(folder != null);
            if (folder.Path == Current?.Path)
                return;

            _oldList.Add(_current);
            _current = folder;
            _newList.Clear();
        }

        private readonly List<IStorageFolder> _newList;
        private readonly List<IStorageFolder> _oldList;
        private IStorageFolder _current;
    }
}