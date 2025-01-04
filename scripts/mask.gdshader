shader_type canvas_item;

uniform sampler2D scene_texture: hint_screen_texture;

void fragment() {
	COLOR = texture(scene_texture, SCREEN_UV);
	COLOR.a = texture(TEXTURE, UV).a;
	COLOR.rgb += vec3(0.1,0.1,0.0).rgb;
	// Place fragment code here.
}
