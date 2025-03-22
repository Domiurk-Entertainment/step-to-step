shader_type canvas_item;

uniform vec4 outline_color = vec4(1.0, 1.0, 1.0, 1.0); // Білий колір обводки
uniform float outline_width = 1.0; // Ширина обводки
uniform float alpha_threshold = 0.5; // Мінімальна прозорість, яку враховуємо

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);

    if (tex_color.a < alpha_threshold) {
        COLOR = vec4(0.0, 0.0, 0.0, 0.0); // Робимо піксель прозорим, а не відкидаємо
    }


    float alpha = tex_color.a;
    vec2 texel_size = 1.0 / vec2(textureSize(TEXTURE, 0));
    float offset = outline_width * texel_size.x;

    float outline = 0.0;

    for (float x = -1.0; x <= 1.0; x += 1.0) {
        for (float y = -1.0; y <= 1.0; y += 1.0) {
            if (x == 0.0 && y == 0.0) continue;
            vec2 sample_uv = UV + vec2(x, y) * offset;
            float sample_alpha = texture(TEXTURE, sample_uv).a;
            if (sample_alpha > alpha_threshold) {
                outline += 1.0;
            }
        }
    }

    if (outline > 0.0 && alpha < alpha_threshold) {
        COLOR = outline_color;
    } else {
        COLOR = tex_color;
    }
}
