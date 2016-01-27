using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Button {
    #region Fields
    //Text
    public string Text { get; set; }
    public SpriteFont Font { get; set; }
    public Color FontColor { get; set; } = Color.Black;
    public int FontSize { get; set; }
    //Image
    public Texture2D SourceTexture { get; set; }
    public Texture2D HoverTexture { get; set; }
    public Texture2D ClickTexture { get; set; }
    public string Tooltip { get; set; }
    //Misc
    public Rectangle Position { get; set; }
    public Vector2 VectorPosition { get { return new Vector2(Position.X, Position.Y); } }
    public Vector2 Origin { get; set; }


    //Drawing
    private bool DrawText = false;

    private bool DrawSourceImage = false;
    private bool DrawHoverTexture = false;
    private bool DrawClickTexture = false;
    private bool DrawHoldTexture = false;
    private bool DrawTooltip = false;

    public float DelayToTooltip;

    public bool Visible = true;
    public bool Enabled = true;
    public Vector2 Scale = new Vector2(1, 1);
    private Vector2 DefaultScale = new Vector2(1, 1);
    public float Rotation = 0.00f;
    public float DefaultRotation = 0.00f;
    #endregion

    #region Constructors

    public Button() { }

    #region Text
    /// <summary>
    /// Default text button with default coloring
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="text">The text you want to display in the button</param>
    /// <param name="font">The font you want the text to have</param>
    public Button(Rectangle position, string text, SpriteFont font) {
        Text = text;
        Font = font;
        Position = position;
        DrawText = true;
    }
    /// <summary>
    /// Advanced text button with default coloring
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="text">The text you want to display in the button</param>
    /// <param name="font">The font you want the text to have</param>
    /// <param name="fontColor">The color you want the text to have</param>
    /// <param name="fontSize">The fontsize you want the text to have</param>
    public Button(Rectangle position, string text, SpriteFont font, Color fontColor, int fontSize) {
        Font = font;
        FontColor = fontColor;
        FontSize = fontSize;
        Text = text;
        Position = position;
        DrawText = true;
    }
    #endregion
    #region Graphics
    /// <summary>
    /// Basic button with just one texture
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    public Button(Rectangle position, Texture2D sourceTexture) {
        Position = position;
        SourceTexture = sourceTexture;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Button with a hover texture
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Button with a hover texture and tooltip
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="toolTip">The text you want displayed when the button gets hovered</param>
    /// <param name="font">The font you want the text to have</param>
    /// <param name="fontColor">The color you want the text to have</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, string toolTip, SpriteFont font, Color fontColor) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        Tooltip = toolTip;
        Font = font;
        FontColor = fontColor;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Default button
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="clickTexture">The texture you want to be displayed when the button gets clicked</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, Texture2D clickTexture) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        ClickTexture = clickTexture;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Default button with tooltip
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="clickTexture">The texture you want to be displayed when the button gets clicked</param>
    /// <param name="toolTip">The text you want displayed when the button gets hovered</param>
    /// <param name="font">The font you want the tooltip to have</param>
    /// <param name="fontColor">The color you want the tooltip to have</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, Texture2D clickTexture, string toolTip, SpriteFont font, Color fontColor) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        ClickTexture = clickTexture;
        Tooltip = toolTip;
        Font = font;
        FontColor = fontColor;
        DrawSourceImage = true;
    }

    #endregion
    #region Both
    /// <summary>
    /// Button with a texture and text
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="text">Text you want to display in the button</param>
    /// <param name="font">Font of the text</param>
    public Button(Rectangle position, Texture2D sourceTexture, string text, SpriteFont font) {
        Position = position;
        SourceTexture = sourceTexture;
        Text = text;
        Font = font;
        DrawText = true;
        DrawSourceImage = true;
    }

    /// <summary>
    /// Button with a normal texture, hover texture and text
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="text">Text you want to display in the button</param>
    /// <param name="font">Font of the text</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, string text, SpriteFont font) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        Text = text;
        Font = font;
        DrawText = true;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Button with a normal texture, hover texture, text and a tooltip
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="toolTip">The text you want displayed when the button gets hovered</param>
    /// <param name="text">Text you want to display in the button</param>
    /// <param name="font">Font of the text</param>
    /// <param name="fontColor">The color you want the text to have</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, string toolTip, string text, SpriteFont font, Color fontColor) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        Tooltip = toolTip;
        Text = text;
        Font = font;
        FontColor = fontColor;
        DrawText = true;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Button a normal texture, hover texture, click texture and text 
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="clickTexture">The texture you want to be displayed when the button gets clicked</param>
    /// <param name="text">Text you want to display in the button</param>
    /// <param name="font">Font of the text</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, Texture2D clickTexture, string text, SpriteFont font) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        ClickTexture = clickTexture;
        DrawSourceImage = true;
        Text = text;
        Font = font;
        DrawText = true;
        DrawSourceImage = true;
    }
    /// <summary>
    /// Button a normal texture, hover texture, click texture and text and with a tooltip
    /// </summary>
    /// <param name="position">The position you want the button to be in</param>
    /// <param name="sourceTexture">The default texture of the button</param>
    /// <param name="hoverTexture">The texture you want to be displayed when the button gets hovered</param>
    /// <param name="clickTexture">The texture you want to be displayed when the button gets clicked</param>
    /// <param name="toolTip">The text you want displayed when the button gets hovered</param>
    /// <param name="text">Text you want to display in the button</param>
    /// <param name="font">Font of the text</param>
    /// <param name="fontColor">The color you want the tooltip to have</param>
    public Button(Rectangle position, Texture2D sourceTexture, Texture2D hoverTexture, Texture2D clickTexture, string toolTip, string text, SpriteFont font, Color fontColor) {
        Position = position;
        SourceTexture = sourceTexture;
        HoverTexture = hoverTexture;
        ClickTexture = clickTexture;
        Tooltip = toolTip;
        DrawSourceImage = true;
        Text = text;
        Font = font;
        FontColor = fontColor;
        DrawText = true;
        DrawSourceImage = true;
    }

    #endregion

    #endregion

    /// <summary>
    /// Updates the drawing states for the buttons, should be run in Update
    /// </summary>
    /// <param name="state">The current state the mouse is in</param>
    /// <param name="prevState">The previous state the mouse was in</param>
    public void Update(MouseState state, MouseState prevState) {
        IsHovering(state);
        IsClicked(state, prevState);
        IsHeld(state);
        IsReleased(state);
    }

    /// <summary>
    /// Draw the button
    /// </summary>
    /// <param name="batch">Spritebatch to draw the button in</param>
    public void Draw(SpriteBatch batch) {
        if (Visible) {
            if (DrawTooltip) {
                batch.DrawString(Font, Tooltip, new Vector2(Position.X, Position.Y), FontColor);
            }
            if (DrawSourceImage) {
                if (Scale == DefaultScale) {
                    batch.Draw(SourceTexture, Position, Color.White);
                }
                else {
                    Origin = new Vector2(SourceTexture.Width / 2 * Scale.X, SourceTexture.Height / 2 * Scale.Y);
                    batch.Draw(SourceTexture, VectorPosition, null, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0.94f);
                }
            }
            if (DrawHoverTexture) {
                batch.Draw(HoverTexture, Position, Color.White);
            }
            if (DrawClickTexture) {
                batch.Draw(ClickTexture, Position, Color.White);
            }
            if (DrawText) {
                var stringsize = Font.MeasureString(Text);
                var pos = new Vector2(Position.X + ((SourceTexture.Width - stringsize.X) / 2), Position.Y + ((SourceTexture.Height - stringsize.Y) / 2));
                batch.DrawString(Font, Text, pos, FontColor);
            }
        }
    }

    /// <summary>
    /// Checks if the cursor is currently hovering the button
    /// </summary>
    /// <param name="state">State of the mouse you want to check</param>
    /// <returns>returns true if the cursor hovers the button</returns>
    public bool IsHovering(MouseState state) {
        var mousePos = new Point(state.X, state.Y);
        if (Position.Contains(mousePos)) {
            if (HoverTexture != null)
                DrawHoverTexture = true;
            if (!string.IsNullOrEmpty(Tooltip)) {
                DrawTooltip = true;
            }
            return true;
        }
        DrawHoverTexture = false;
        return false;
    }
    /// <summary>
    /// Checks if the mouse was clicked for a short moment
    /// </summary>
    /// <param name="state">State of the mouse you want to check</param>
    /// <param name="previousState">Previous state of the mouse you want to check</param>
    /// <returns>returns true if button is clicked</returns>
    public bool IsClicked(MouseState state, MouseState previousState) {
        if (IsHovering(state)) {
            if (state.LeftButton == ButtonState.Pressed && IsReleased(previousState)) {
                if (ClickTexture != null)
                    DrawClickTexture = true;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Checks if the button is being held down
    /// </summary>
    /// <param name="state">State of the mouse you want to check</param>
    /// <returns>returns true if the button is held down</returns>
    public bool IsHeld(MouseState state) {
        if (IsHovering(state)) {
            if (state.LeftButton == ButtonState.Pressed) {
                if (ClickTexture != null)
                    DrawClickTexture = true;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the button has been released
    /// </summary>
    /// <param name="state">State of the mouse you want to check</param>
    /// <returns>returns true if the button is released</returns>
    public bool IsReleased(MouseState state) {
        if (IsHovering(state)) {
            if (state.LeftButton == ButtonState.Released) {
                DrawClickTexture = false;
                return true;
            }
        }
        return false;
    }

}

