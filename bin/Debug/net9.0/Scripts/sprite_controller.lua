-- Sprite Controller Script
-- This script controls the sprite's movement and rotation

local time = 0
local radius = 100
local centerX = 400
local centerY = 300
local speed = 2 -- Rotation speed in radians per second

function init()
	print("Sprite controller initialized!")

	local transform = instance.GetTransform()
	if transform then
		-- Set origin to center of texture (25, 25 for a 50x50 texture)
		transform.Origin = vec2(25, 25)
		print("Transform origin set to center")
	else
		print("Warning: No Transform2D component found!")
	end
end

function update(dt)
	time = time + dt * speed

	local transform = instance.GetTransform()
	if transform then
		-- Move in a circular path
		local x = centerX + math.cos(time) * radius
		local y = centerY + math.sin(time) * radius
		transform.Position = vec2(x, y)

		-- Rotate the sprite as it moves
		transform.Rotation = time

		-- Optional: Scale pulsing effect
		local scaleValue = 1.0 + math.sin(time * 2) * 0.2
		transform.Scale = vec2(scaleValue, scaleValue)
	end
end
