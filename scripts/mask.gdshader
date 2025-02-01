shader_type canvas_item;

uniform sampler2D scene_texture: hint_screen_texture;

void fragment() {
	// Отримуємо піксель з екранної текстури
    vec4 scene_color = texture(scene_texture, SCREEN_UV);

    // Отримуємо альфа-канал з текстури маски
    float mask_alpha = texture(TEXTURE, UV).a;

    // Якщо піксель повністю прозорий у масці, видаляємо його
    if (mask_alpha < 0.1) {
        discard;
    }

    // Формуємо підсумковий колір
    COLOR = scene_color;
    COLOR.a = mask_alpha;
}
