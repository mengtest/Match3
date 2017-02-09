using DavidOchmann.Animation;
using UnityEditor;
using UnityEngine;

namespace DavidOchmann.CustomEditorTools
{
    /**
	 * Updated Interface
	 */

    public interface IUpdate
    {
        void Update();
    }

    /**
	 * EditorButton
	 */

    public class EditorButton : IUpdate
    {
        private string name;

        public EditorButton(string name)
        {
            this.name = name;
        }

        public event OnClickEventHandler OnClick;

        public delegate void OnClickEventHandler(EditorButton editorButton);

        protected virtual void InvokeClick()
        {
            if (OnClick != null) OnClick(this);
        }

        public void Update()
        {
            bool clicked = GUILayout.Button(name);

            if (clicked)
                InvokeClick();
        }
    }

    /**
	 * EditorPopup
	 */

    public class EditorPopup : IUpdate
    {
        public PopupVO popupVO;
        public GUILayoutOption[] guiLayoutOptions;

        public EditorPopup(PopupVO popupVO, GUILayoutOption[] guiLayoutOptions = null)
        {
            this.popupVO = popupVO;
            this.guiLayoutOptions = guiLayoutOptions;
        }

        public event OnChangeEventHandler OnChange;

        public delegate void OnChangeEventHandler(EditorPopup editorPopup);

        protected virtual void InvokeChange()
        {
            if (OnChange != null) OnChange(this);
        }

        /**
		 * Getter / Setter
		 */

        public int index
        {
            get
            {
                return popupVO.index;
            }
        }

        public string value
        {
            get
            {
                return popupVO.list[popupVO.index];
            }
        }

        /**
		 * Public
		 */

        public void Update()
        {
            int value = EditorGUILayout.Popup(popupVO.name, popupVO.index, popupVO.list.ToArray(), guiLayoutOptions);

            if (value != popupVO.index)
            {
                popupVO.index = value;
                InvokeChange();
            }
        }
    }

    /**
	 * PopupFloatField
	 */

    public class PopupFloatField : IUpdate
    {
        public PopupFloatFieldVO popupFloatFieldVO;

        // public List<string> list;
        // public float value;
        public EditorPopup editorPopup;

        public EditorButton editorButton;

        public PopupFloatField(PopupFloatFieldVO popupFloatFieldVO)
        {
            this.popupFloatFieldVO = popupFloatFieldVO;
            // this.list = list;
            // this.value = value;

            init();
        }

        /**
		 * Events
		 */

        public event OnCloseEventHandler OnClose;

        public delegate void OnCloseEventHandler(PopupFloatField popupFloatField);

        protected virtual void InvokeClose()
        {
            if (OnClose != null) OnClose(this);
        }

        /**
		 * Public
		 */

        public void Update()
        {
            EditorGUILayout.BeginHorizontal();

            editorPopup.Update();
            updateFloatField();
            editorButton.Update();

            EditorGUILayout.EndHorizontal();
        }

        /**
		 * Private
		 */

        private void init()
        {
            initEditorPopup();
            initEditorButton();
        }

        /** EditorPopup instance. */

        private void initEditorPopup()
        {
            GUILayoutOption[] guiLayoutOptions =
            {
                GUILayout.MinWidth( 100 )
            };

            PopupVO popupVO = new PopupVO();

            popupVO.name = "";
            popupVO.index = popupFloatFieldVO.index;
            popupVO.list = popupFloatFieldVO.list;

            editorPopup = new EditorPopup(popupVO, guiLayoutOptions);
            editorPopup.OnChange += editorPopupOnChangeHandler;
        }

        private void editorPopupOnChangeHandler(EditorPopup editorPopup)
        {
            popupFloatFieldVO.index = editorPopup.index;
        }

        /** Label and FloatField. */

        private void updateFloatField()
        {
            EditorGUILayout.LabelField("value:", GUILayout.Width(34));
            popupFloatFieldVO.value = EditorGUILayout.FloatField(popupFloatFieldVO.value);
        }

        /** EditorButtonInstance. */

        private void initEditorButton()
        {
            editorButton = new EditorButton("x");
            editorButton.OnClick += editorButtonOnClickHandler;
        }

        private void editorButtonOnClickHandler(EditorButton editorButton)
        {
            InvokeClose();
        }
    }
}