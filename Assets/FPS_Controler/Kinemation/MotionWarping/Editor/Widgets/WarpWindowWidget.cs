// Designed by KINEMATION, 2024.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KINEMATION.MotionWarping.Editor.Widgets
{
    public class WarpWindowWidget : IWarpWidgetInterface
    {
        class DraggableArea
        {
            private const float MinAreaWidth = 10f;
            private const float BorderTolerance = 5f;
            private const float BorderWidth = 2f;

            public Rect Parent;
            public float LocalStart;
            public float LocalEnd;

            private Color _color;

            // -2: not hovered, -1: left border, 0: body, 1: right border.
            public int GetHoveredPart(Vector2 mousePosition)
            {
                if (mousePosition.x >= GetRange().Item1 - BorderTolerance 
                    && mousePosition.x <= GetRange().Item1 + BorderTolerance)
                {
                    return -1;
                }

                if (mousePosition.x >= GetRange().Item2 - BorderTolerance 
                    && mousePosition.x <= GetRange().Item2 + BorderTolerance)
                {
                    return 1;
                }

                return 0;
            }

            public DraggableArea(float start, float end, Rect parent)
            {
                LocalStart = start;
                LocalEnd = end;
                _color = new Color(0f,0.5f,0.5f);
                this.Parent = parent;
            }

            public float GetThickness()
            {
                float worldStart = Mathf.Lerp(Parent.xMin, Parent.xMax, LocalStart);
                float worldEnd = Mathf.Lerp(Parent.xMin, Parent.xMax, LocalEnd);

                return worldEnd - worldStart;
            }

            public Rect GetRect()
            {
                Rect rect = Parent;
                
                rect.x = GetRange().Item1;
                rect.width = GetThickness();
                
                return rect;
            }

            public (float, float) GetRange()
            {
                float worldStart = Mathf.LerpUnclamped(Parent.xMin, Parent.xMax, LocalStart);
                float worldEnd = Mathf.LerpUnclamped(Parent.xMin, Parent.xMax, LocalEnd);

                return (worldStart, worldEnd);
            }

            public float GetLocal(float value)
            {
                if (Mathf.Approximately(Parent.xMin, Parent.xMax)) return 0f;

                return (value - Parent.xMin) / (Parent.xMax - Parent.xMin);
            }

            public void RenderArea(float opacity)
            {
                // Draw body.
                Rect areaRect = Parent;
                areaRect.x = GetRange().Item1;
                areaRect.width = GetThickness();
                
                _color.a = opacity;
                EditorGUI.DrawRect(areaRect, _color);
                
                // Draw borders.
                areaRect.x -= BorderWidth / 2f;
                areaRect.width = BorderWidth;
                EditorGUI.DrawRect(areaRect, new Color(1f, 1f, 1f, opacity));

                areaRect.x = GetRange().Item2 - BorderWidth / 2f;
                EditorGUI.DrawRect(areaRect, new Color(1f, 1f, 1f, opacity));
            }

            public bool Contains(Vector2 checkPosition)
            {
                bool x = checkPosition.x >= GetRange().Item1 && checkPosition.x <= GetRange().Item2;
                bool y = checkPosition.y >= Parent.yMin && checkPosition.y <= Parent.yMax;

                return x && y;
            }

            public void Resize(float mouseDelta, int part)
            {
                // Cache the values
                float start = LocalStart;
                float end = LocalEnd;

                float left = GetRange().Item1;
                float right = GetRange().Item2;

                if (part == -1)
                {
                    left -= part * mouseDelta;
                }

                if (part == 0)
                {
                    left += mouseDelta;
                    right += mouseDelta;
                }

                if (part == 1)
                {
                    right += mouseDelta;
                }

                LocalStart = GetLocal(left);
                LocalEnd = GetLocal(right);

                if (GetThickness() - BorderTolerance * 2f < MinAreaWidth)
                {
                    LocalStart = start;
                    LocalEnd = end;
                }
            }
        }
        
        public delegate void WarpWindowCallback(int modifiedArea);
        public WarpWindowCallback OnAreaModified;
        
        private const float TimelineHeight = 20f;
        
        private const float PlaybackTolerance = 8f;
        private const float PlaybackWidth = 2f;

        private Rect _timelineRect;
        private List<DraggableArea> _draggableAreas = new List<DraggableArea>();

        private DraggableArea _activeArea;
        private int _resizeAction;
        private bool _mousePressed;

        private float _playbackPosition;
        private bool _movingPlayback;

        private bool IsAreaColliding(float proposedPosition, int areaIndex)
        {
            int areasCount = _draggableAreas.Count;

            if (areasCount == 0)
            {
                return false;
            }

            int leftIndex = areaIndex - 1;
            int rightIndex = areaIndex + 1;

            // Check left border
            var leftBorderCollision = proposedPosition <
                                      (leftIndex < 0 ? _timelineRect.xMin : _draggableAreas[leftIndex].GetRange().Item2);

            // Check right border
            var rightBorderCollision = proposedPosition >
                                       (rightIndex > areasCount - 1
                                           ? _timelineRect.xMax
                                           : _draggableAreas[rightIndex].GetRange().Item1);

            return leftBorderCollision || rightBorderCollision;
        }

        private void UpdateArea(int areaIndex, bool mouseAction)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 mouseDelta = Event.current.delta;
            DraggableArea area = _draggableAreas[areaIndex];

            area.Parent = _timelineRect;
            
            // Enable editing if the cursor is within the bounds.
            if (mouseAction && _mousePressed && area.Contains(mousePosition))
            {
                _activeArea = area;
                _resizeAction = area.GetHoveredPart(mousePosition);
                Event.current.Use();
            }

            if (_activeArea != null && mouseAction && !_mousePressed)
            {
                _activeArea = null;
                _resizeAction = -2;
                Event.current.Use();
            }
            
            if (_activeArea == null || area != _activeArea)
            {
                area.RenderArea(1f);
                return;
            }
            
            RenderCursorIcon(area, _resizeAction);

            if (Event.current.type == EventType.MouseDrag)
            {
                // Cache the area current size
                float areaStart = area.LocalStart;
                float areaEnd = area.LocalEnd;

                // Resize the area
                area.Resize(mouseDelta.x, _resizeAction);

                float start = area.GetRange().Item1;
                float end = area.GetRange().Item2;

                bool collideLeft = IsAreaColliding(start, areaIndex);
                bool collideRight = IsAreaColliding(end, areaIndex);

                // Check for any collisions
                if (collideLeft || collideRight)
                {
                    area.LocalStart = areaStart;
                    area.LocalEnd = areaEnd;
                }
                else
                {
                    OnAreaModified?.Invoke(areaIndex);
                }

                Event.current.Use();
            }
            
            area.RenderArea(0.7f);
        }

        private void RenderCursorIcon(DraggableArea area, int hoveredPart)
        {
            if (hoveredPart == 0)
            {
                EditorGUIUtility.AddCursorRect(area.GetRect(), MouseCursor.Pan);
                return;
            }

            EditorGUIUtility.AddCursorRect(area.GetRect(), MouseCursor.SlideArrow);
        }
        
        private void UpdateDraggableAreas()
        {
            bool prevMousePressed = _mousePressed;

            if (Event.current.type == EventType.MouseDown)
            {
                _mousePressed = true;
            }
            
            if (Event.current.type == EventType.MouseUp)
            {
                _mousePressed = false;
            }
            
            for (int i = 0; i < _draggableAreas.Count; i++)
            {
                UpdateArea(i, prevMousePressed != _mousePressed);
            }
        }

        public void ClearPhases()
        {
            _draggableAreas.Clear();
        }

        public void AddWarpPhase(float start, float end)
        {
            DraggableArea newArea = new DraggableArea(start, end, _timelineRect);
            _draggableAreas.Add(newArea);
        }

        public (float, float) GetAreaSize(int areaIndex)
        {
            (float, float) size = (0f, 0f);

            if (_draggableAreas.Count == 0 || areaIndex < 0 || areaIndex > _draggableAreas.Count - 1) return size;

            var area = _draggableAreas[areaIndex];
            
            size.Item1 = area.LocalStart;
            size.Item2 = area.LocalEnd;
            
            return size;
        }

        private void RenderPlayback(bool bAction = false)
        {
            float worldPlayback = Mathf.Lerp(_timelineRect.xMin, _timelineRect.xMax, _playbackPosition);
            var playbackRect = _timelineRect;
            playbackRect.y -= TimelineHeight;
            
            Vector2 mousePosition = Event.current.mousePosition;
            
            if (bAction && _mousePressed && playbackRect.Contains(mousePosition))
            {
                if (mousePosition.x >= worldPlayback - PlaybackTolerance
                    && mousePosition.x <= worldPlayback + PlaybackTolerance)
                {
                    _movingPlayback = true;
                }
            }

            if (bAction && !_mousePressed)
            {
                _movingPlayback = false;
            }

            if (_movingPlayback)
            {
                playbackRect.height *= 2f;
                EditorGUIUtility.AddCursorRect(playbackRect, MouseCursor.SlideArrow);
                playbackRect.height /= 2f;
                if (Event.current.type == EventType.MouseDrag)
                {
                    worldPlayback += Event.current.delta.x;
                    _playbackPosition = Mathf.InverseLerp(_timelineRect.xMin, _timelineRect.xMax, worldPlayback);
                    Event.current.Use();
                }
            }
            
            if(_activeArea != null && Event.current.shift)
            {
                if (_resizeAction is 0 or -1)
                {
                    _playbackPosition = _activeArea.LocalStart;
                }

                if (_resizeAction == 1)
                {
                    _playbackPosition = _activeArea.LocalEnd;
                }
            }
            
            playbackRect.x = Mathf.Lerp(_timelineRect.xMin, _timelineRect.xMax, _playbackPosition);
            playbackRect.x -= PlaybackWidth / 2f;
            playbackRect.width = PlaybackWidth;
            playbackRect.height = _timelineRect.yMax - playbackRect.y;
            
            EditorGUI.DrawRect(playbackRect, Color.red);
        }

        public float GetPlayback()
        {
            return _playbackPosition;
        }

        public void Render()
        {
            float width = EditorGUIUtility.currentViewWidth;
            
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(width, TimelineHeight), new Color(0.15f, 0.15f, 0.15f));

            var cacheRect = _timelineRect; 
            _timelineRect = GUILayoutUtility.GetRect(width, TimelineHeight);

            if (Mathf.Approximately(_timelineRect.width, 1f))
            {
                _timelineRect = cacheRect;
            }
            
            EditorGUI.DrawRect(_timelineRect, new Color(0.15f, 0.15f, 0.15f));
            bool action = _mousePressed;
            UpdateDraggableAreas();
            RenderPlayback(action != _mousePressed);
        }
    }
}