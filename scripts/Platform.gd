extends Spatial


var _tween: Tween
var _border: MeshInstance

var _end: bool
var _result: bool


func _ready():
	_tween = get_node("Tween")
	_border = get_node("Border")

	_recolor()


func play_fade_out_animation():
	_end = true
	var material = _border.get_surface_material(0) as SpatialMaterial

	_result = _tween.stop_all()
	_play_tween_animation(material, 1, 0)


func _recolor():
	var material: SpatialMaterial = Globals.get_emission_material(0)
	material.emission_energy = 0

	_border.set_surface_material(0, material)
	_play_tween_animation(material, 0, 1)


func _play_tween_animation(material, startValue, endValue):
	var speed = Globals.animation_speed
	speed -= 0.05

	_result = _tween.interpolate_property(material, "emission_energy",
			startValue, endValue, speed, Tween.TRANS_SINE)
	_result = _tween.start()


func _on_tween_completed():
	if _end:
		self.get_parent().queue_free()
