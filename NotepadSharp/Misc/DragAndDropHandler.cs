using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class DragAndDropHandler {
        bool _draggingStarted;
        Func<string, bool> _allowDrop;
        Func<bool> _allowDrag;
        Action<string> _drop;
        Func<DataObject> _makeDragObject;

        public DragAndDropHandler(Func<bool> allowDrag, Func<DataObject> makeDragObject) : this(x => false, x => { }, allowDrag, makeDragObject) { }
        public DragAndDropHandler(Func<string, bool> allowDrop, Action<string> drop) : this(allowDrop, drop, () => false, () => null) { }

        public DragAndDropHandler(Func<string, bool> allowDrop, Action<string> drop, Func<bool> allowDrag, Func<DataObject> makeDragObject) {
            _allowDrop = allowDrop;
            _allowDrag = allowDrag;
            _drop = drop;
            _makeDragObject = makeDragObject;

            DropCommand = new RelayCommand(x => Drop((DragEventArgs)x));
            DragOverCommand = new RelayCommand(x => DragOver((DragEventArgs)x));
            MouseLeftButtonDownCommand = new RelayCommand(x => _draggingStarted = true, x => _allowDrag());
            MouseMoveCommand = new RelayCommand(x => HandleDrag((MouseEventArgs)x), x => _draggingStarted);
        }

        public ICommand DropCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseMoveCommand { get; }

        protected virtual void Drop(DragEventArgs e) {
            _drop(GetDropPath(e));
            e.Handled = true;
        }

        private void HandleDrag(MouseEventArgs x) {
            _draggingStarted = false;
            if (x.LeftButton == MouseButtonState.Pressed) {
                DragDrop.DoDragDrop((DependencyObject)x.Source, _makeDragObject(), DragDropEffects.All);
            }
        }

        private string GetDropPath(DragEventArgs args) {
            var files = (string[])args.Data.GetData(DataFormats.FileDrop);
            return (files?.Length ?? 0) == 0 ? "" : files[0];
        }

        private void DragOver(DragEventArgs e) {
            var allow = _allowDrop(GetDropPath(e));
            e.Effects = allow ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }
    }
}
