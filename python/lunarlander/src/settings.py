class Settings:
	"""These are game settings.  You can change these to adjust the dynamics of the game."""
	DEBUG = False

	SCALE = .75
	SCREEN_WIDTH = 1920 * SCALE
	SCREEN_HEIGHT = 1080 * SCALE
	LANDER_WIDTH = 50 * SCALE
	LANDER_HEIGHT = 50 * SCALE
	LANDER_FEET_YOFFSET = LANDER_HEIGHT
	BURNER_PRIMARY_SIZE = 50 * SCALE
	BURNER_SECONDARY_SIZE = 20 * SCALE
	BURNER_PRIMARY_XOFFSET  = LANDER_WIDTH / 2.0 - BURNER_PRIMARY_SIZE / 2.0 
	BURNER_PRIMARY_yOFFSET  = LANDER_HEIGHT - BURNER_PRIMARY_SIZE / 4.0
	BURNER_SECONDARY_LEFT_XOFFSET = 0 - BURNER_SECONDARY_SIZE + BURNER_SECONDARY_SIZE / 5.0
	BURNER_SECONDARY_LEFT_YOFFSET = LANDER_HEIGHT / 2.0 - BURNER_SECONDARY_SIZE / 2.0
	BURNER_SECONDARY_RIGHT_XOFFSET = LANDER_WIDTH - BURNER_SECONDARY_SIZE / 2.5
	BURNER_SECONDARY_RIGHT_YOFFSET = LANDER_HEIGHT / 2.0 - BURNER_SECONDARY_SIZE / 2.0

	LANDINGPAD_COORDS = (835 * SCALE, 783 * SCALE)
	LANDINGPAD_SIZE = (185 * SCALE, 192 * SCALE)
	LANDINGPAD_RECT = (LANDINGPAD_COORDS[0], LANDINGPAD_COORDS[1], LANDINGPAD_SIZE[0], LANDINGPAD_SIZE[1])
	LANDINGPAD_LINE_Y = 862 * SCALE
	LANDINGPAD_X_BOUNDS = (LANDINGPAD_COORDS[0], LANDINGPAD_COORDS[0] + LANDINGPAD_SIZE[0])
	SHRAPNEL_SIZE = 20
	
	CRASH_FIRES = [(0, 0, 0.1, 3.14), (10, 10, 0.25, 3.14)] 

	# This is a special number in physics.  It means that gravity of a planet will accelerate an object
	# toward it at a rate of 9.8 meters per second per second.  However, it is the gravity of Earth!
	# We will adjust it to be moon gravity using GRAVITY_SCALE.
	GRAVITY = 9.8
	GRAVITY_SCALE = 0.005
	PRIMARY_THRUST = GRAVITY * 2
	LEFT_THRUST = 3.0
	RIGHT_THRUST= 3.0
	THRUST_SCALE = 0.005
	MAX_LANDING_VELOCITY = 2.0
	METERS_PER_PIXEL = 10.0
	INITIAL_FUEL = 300.0
	FUEL_CONSUMPTION = 5.0
	INITIAL_VELOCITY = (0, 0)

class Paths:
	"""These are the locations on disk of all the game assets, such as sounds and graphics."""
	GRAPHICS_BACKGROUND = "./assets/graphics/background.png"
	GRAPHICS_BURNER = "./assets/graphics/burner.png"
	GRAPHICS_LANDER = "./assets/graphics/lander.png"
	GRAPHICS_LANDINGPAD = "./assets/graphics/landingpad.png"
	GRAPHICS_TITLESCREEN = "./assets/graphics/title-screen.png"
	GRAPHICS_SHRAPNEL = "./assets/graphics/shrapnel.png"

	FONT_PATH = "./assets/fonts/eurostile-extended-regular.ttf"

	SOUND_BURNER = "./assets/sounds/burner.wav"