Shader "Unlit/GhostShader"
{
    Properties{

        // Outline properties
        // Base outline colour
        _OutlineColour("Outline Colour", Color) = (0.95,0.5,0.5,0.5)

        // Base outline width
        _OutlineWidth("Outline Width", Float) = 0.01

        // The maximum amount by which vertices are randomly moved
        _Scribbliness("Scribbliness", Float) = 0.01

        // The rate at which vertices are moved (per second)
        // 0.0 = not animated
        _RedrawRate("Outline Redraw Rate", Float) = 3.0

        // Fill properties
        _MainTex("Base (RGB)", 2D) = "white" {} // The base material texture
        _FillTex("Fill Texture RGBA", 2D) = "white" {}  // Six levels of fill texture, increasing in density    
        _FillColour("Fill Colour (A=Opacity)", Color) = (0.1607, 0.1607, 0.7922, 1)   // Fill tint colour
        _IntensityModifier("Intensity Modifier", Range(-1.0,1.0)) = 0.0 // Modifier to increase brightness (i.e. to lighten texture density)
        _RedrawRate("Fill Redraw Rate", float) = 3.0 // How many times per second should the fill be redrawn                                                    
    }
        SubShader{
            Tags {
                "Queue" = "Geometry+100" // Render this object after geometry queue but before transparent objects
                "RenderType" = "Opaque" // The material is opaque
                "IgnoreProjector" = "True" // Don't let projectors affect this object
            }

            // Use "Multiply" blend
            Blend DstColor Zero

            // Outline
            UsePass "Hand-Drawn/Outline/Simple/INTERIORMASK"
            //UsePass "Hand-Drawn/Fill/SmoothedGreyscale/INTERIOR"
            UsePass "Hand-Drawn/Outline/Simple/OUTLINE"

            // Fill
            UsePass  "Hand-Drawn/Fill/MultiTextured(Multiply)/FORWARD"
            UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE1"
            UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE2"
            UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE3"
            UsePass "Hand-Drawn/Outline/Overdrawn/OUTLINE4"




        }
}
