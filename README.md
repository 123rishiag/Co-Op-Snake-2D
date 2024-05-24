# Co-Op Snake 2D

[Gameplay Video](https://www.loom.com/share/40a3ddbdbeff4dafb783bdcd49f2e287?sid=d788edd2-331a-4785-8577-85a8c434aee7)

## Overview

A modern take on the classic snake game, with added features like power-ups, flexible game parameters, and a cooperative mode for two players. The goal is to navigate your snake, eat food, grow longer, and avoid biting yourself or colliding with another player’s snake.

## Features

- **Core Snake Functionality:**
  - Move the snake in all four directions using WASD (Player 1) and arrow keys (Player 2).
  - Screen wrapping for continuous play.
  - Snake growth upon eating food.
  - Self-collision detection leading to game over.

- **Power-Ups:**
  - **Shield:** Grants temporary immunity from death and prevents score reduction.
  - **Score Boost:** Doubles the score for each food eaten.
  - **Speed Up:** Increases the snake's speed for a short duration.
  - Power-ups despawn after a certain time if not collected.

- **Foods:**
  - **Mass Gainer:** Increases the length of the snake.
  - **Mass Burner:** Decreases the length of the snake but does not reduce length if the snake is already at or below a certain low length.
  - Foods despawn after a certain time if not eaten.

- **Co-Op Mode:**
  - Two-player mode with separate controls.
  - Collision between snakes results in one snake dying.

- **Scoring:**
  - Score increases with Mass Gainer food.
  - Score decreases with Mass Burner food.

- **UI:**
  - Basic UI with Score display, Pause/Resume (Esc), Restart, and Quit buttons.
  - In-game Game Over menu shows the winner.

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/123rishiag/Co-Op-Snake-2D.git
