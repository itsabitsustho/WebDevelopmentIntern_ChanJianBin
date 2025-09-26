const recipes = [
      {
        id: 'som-tum',
        country: 'thailand',
        title: 'Som Tum (Green Papaya Salad)',
        icon: '🥗',
        description: 'A refreshing Thai salad that balances sweet, sour, salty, and spicy flavors perfectly.',
        difficulty: 'Easy',
        prepTime: 15,
        cookTime: 0,
        servings: 2,
        calories: 120,
        benefits: 'Rich in digestive enzymes and vitamin C, supports metabolism and immune system.',
        ingredients: [
          '2 cups green papaya, shredded',
          '2 cloves garlic, minced',
          '2-3 Thai chilies (adjust to taste)',
          '2 tbsp lime juice',
          '1 tbsp palm sugar or brown sugar',
          '1 tbsp fish sauce (or soy sauce for vegetarian)',
          '1/4 cup roasted peanuts, crushed',
          '1 medium tomato, sliced',
          '2 tbsp green beans, cut into 1-inch pieces',
          'Fresh herbs for garnish'
        ],
        instructions: [
          'Pound garlic and chilies in a large mortar and pestle until fragrant.',
          'Add palm sugar and continue pounding to dissolve.',
          'Add lime juice and fish sauce, mix well.',
          'Add shredded papaya and pound gently to bruise and mix with dressing.',
          'Add tomatoes, green beans, and half the peanuts. Pound briefly to combine.',
          'Taste and adjust seasonings - should be balanced between sweet, sour, salty, and spicy.',
          'Transfer to serving plate and garnish with remaining peanuts and fresh herbs.',
          'Serve immediately with fresh vegetables like cabbage and cucumber.'
        ],
        nutrition: {
          protein: '4g',
          carbs: '18g',
          fiber: '6g',
          sugar: '12g',
          sodium: '680mg',
          vitaminC: '85mg'
        }
      },
      {
        id: 'ulam-rice',
        country: 'malaysia',
        title: 'Ulam Rice with Grilled Fish',
        icon: '🌱',
        description: 'Traditional Malaysian herb salad that provides natural detoxification and nutrients.',
        difficulty: 'Medium',
        prepTime: 20,
        cookTime: 15,
        servings: 3,
        calories: 280,
        benefits: 'Antioxidant-rich herbs support liver detox, while fresh vegetables provide essential vitamins.',
        ingredients: [
          '1 cup cooked brown rice',
          '200g white fish fillet',
          '1 cup young coconut shoots, sliced',
          '1 cup cabbage, shredded',
          '1/2 cup long beans, sliced',
          '1/4 cup mint leaves',
          '1/4 cup Vietnamese coriander',
          '2 tbsp tamarind paste',
          '2 tbsp palm sugar',
          '2 tbsp water',
          '2 shallots, sliced',
          '2 chilies, sliced'
        ],
        instructions: [
          'Season fish with salt and pepper, then grill until cooked through.',
          'Mix tamarind paste, palm sugar, and water to make dressing.',
          'Combine all vegetables and herbs in a large bowl.',
          'Add the tamarind dressing and toss gently.',
          'Serve ulam over rice with grilled fish on the side.',
          'Garnish with sliced shallots and chilies.',
          'Eat with your hands for the authentic experience!'
        ],
        nutrition: {
          protein: '25g',
          carbs: '32g',
          fiber: '8g',
          sugar: '8g',
          sodium: '420mg',
          omega3: '850mg'
        }
      },
      {
        id: 'thunder-tea',
        country: 'singapore',
        title: 'Thunder Tea Rice (Lei Cha Fan)',
        icon: '🍚',
        description: 'Hakka herb tea rice that combines nutrition with traditional Chinese medicine principles.',
        difficulty: 'Medium',
        prepTime: 30,
        cookTime: 20,
        servings: 4,
        calories: 320,
        benefits: 'Herbal tea aids digestion and provides antioxidants, while mixed vegetables ensure balanced nutrition.',
        ingredients: [
          '2 cups cooked brown rice',
          '1/4 cup green tea leaves',
          '1/4 cup mint leaves',
          '2 tbsp sesame seeds',
          '2 tbsp peanuts',
          '1 cup spinach, blanched',
          '1 cup bean sprouts, blanched',
          '1/2 cup diced tofu, pan-fried',
          '1/4 cup pickled radish',
          '2 tbsp soy sauce',
          'Salt and white pepper to taste'
        ],
        instructions: [
          'Grind tea leaves, mint, sesame seeds, and peanuts in a mortar until fine.',
          'Add hot water gradually to form a green tea paste.',
          'Strain the mixture to get smooth green tea soup.',
          'Season with salt and white pepper.',
          'Arrange rice in bowls with blanched vegetables and tofu.',
          'Pour the hot green tea soup over the rice.',
          'Add pickled radish and a dash of soy sauce.',
          'Mix everything together before eating.'
        ],
        nutrition: {
          protein: '18g',
          carbs: '45g',
          fiber: '12g',
          sugar: '4g',
          sodium: '580mg',
          antioxidants: 'High'
        }
      },
      {
        id: 'gado-gado',
        country: 'indonesia',
        title: 'Gado-Gado',
        icon: '🥗',
        description: 'Indonesian mixed vegetable salad with rich peanut sauce - a complete plant-based meal.',
        difficulty: 'Medium',
        prepTime: 25,
        cookTime: 15,
        servings: 3,
        calories: 350,
        benefits: 'Complete plant-based protein from tempeh and peanuts, high fiber supports digestive health.',
        ingredients: [
          '1 block tempeh, sliced and fried',
          '2 eggs, hard-boiled',
          '1 cup green beans, blanched',
          '1 cup bean sprouts, blanched',
          '1 cup spinach, blanched',
          '1 cucumber, sliced',
          '1 cup cabbage, shredded',
          '1/2 cup roasted peanuts',
          '2 cloves garlic',
          '2 red chilies',
          '2 tbsp palm sugar',
          '1 tbsp tamarind paste',
          '1 tsp salt',
          'Keropok (crackers) for serving'
        ],
        instructions: [
          'Grind peanuts, garlic, and chilies into a paste.',
          'Add palm sugar, tamarind paste, and salt. Mix well.',
          'Add water gradually to achieve desired sauce consistency.',
          'Arrange all vegetables, tempeh, and halved eggs on a plate.',
          'Drizzle generously with peanut sauce.',
          'Serve with keropok crackers on the side.',
          'Mix everything together before eating.'
        ],
        nutrition: {
          protein: '22g',
          carbs: '28g',
          fiber: '15g',
          sugar: '12g',
          sodium: '720mg',
          vitaminK: '180mcg'
        }
      },
      {
        id: 'pho-ga',
        country: 'vietnam',
        title: 'Pho Ga (Chicken Pho)',
        icon: '🍜',
        description: 'Vietnamese chicken noodle soup with aromatic broth and fresh herbs.',
        difficulty: 'Hard',
        prepTime: 30,
        cookTime: 120,
        servings: 4,
        calories: 380,
        benefits: 'Collagen-rich bone broth supports joint health, while fresh herbs provide vitamins and antioxidants.',
        ingredients: [
          '1 whole chicken (1.5kg)',
          '200g rice noodles',
          '1 onion, charred',
          '5cm ginger, charred',
          '2 star anise',
          '1 cinnamon stick',
          '3 cloves',
          '1 tbsp coriander seeds',
          '1 tbsp fish sauce',
          '1 tsp salt',
          'Fresh herbs: cilantro, Thai basil, mint',
          '1 lime, cut in wedges',
          '2 chilies, sliced',
          '1 onion, thinly sliced'
        ],
        instructions: [
          'Char onion and ginger over open flame until fragrant.',
          'Toast spices in dry pan until aromatic.',
          'Boil chicken in large pot with charred vegetables and spices for 1.5 hours.',
          'Remove chicken, shred meat, and strain broth.',
          'Season broth with fish sauce and salt.',
          'Soak rice noodles until soft.',
          'Place noodles and chicken in bowls.',
          'Pour hot broth over noodles.',
          'Serve with fresh herbs, lime, and chilies.',
          'Add herbs and condiments to taste.'
        ],
        nutrition: {
          protein: '35g',
          carbs: '42g',
          fiber: '3g',
          sugar: '6g',
          sodium: '980mg',
          collagen: 'High'
        }
      },
      {
        id: 'sinigang',
        country: 'philippines',
        title: 'Sinigang na Hipon (Shrimp Sour Soup)',
        icon: '🍲',
        description: 'Filipino tamarind-based soup packed with vegetables and lean protein.',
        difficulty: 'Easy',
        prepTime: 15,
        cookTime: 25,
        servings: 4,
        calories: 180,
        benefits: 'High in vitamin C from tamarind, low in calories but rich in minerals from seafood and vegetables.',
        ingredients: [
          '500g medium shrimp, peeled',
          '2 tbsp tamarind paste',
          '1 onion, quartered',
          '2 tomatoes, quartered',
          '1 cup kangkong (water spinach)',
          '1 cup string beans, cut in 2-inch pieces',
          '1 radish, sliced',
          '2 long green chilies',
          '2 tbsp fish sauce',
          '6 cups water',
          'Salt and pepper to taste'
        ],
        instructions: [
          'Boil water in a large pot.',
          'Add onions and tomatoes, cook until soft.',
          'Add tamarind paste and stir to dissolve.',
          'Add radish and string beans, cook for 5 minutes.',
          'Add shrimp and cook until pink and cooked through.',
          'Season with fish sauce, salt, and pepper.',
          'Add kangkong and chilies in the last 2 minutes.',
          'Serve hot with steamed rice.'
        ],
        nutrition: {
          protein: '28g',
          carbs: '12g',
          fiber: '6g',
          sugar: '8g',
          sodium: '850mg',
          vitaminC: '45mg'
        }
      }
    ];

    let currentFilter = 'all';

    function renderRecipes(filter = 'all') {
      const grid = document.getElementById('recipesGrid');
      grid.innerHTML = '';

      const filteredRecipes = filter === 'all' 
        ? recipes 
        : recipes.filter(recipe => recipe.country === filter);

      filteredRecipes.forEach((recipe, index) => {
        const recipeCard = document.createElement('div');
        recipeCard.className = 'recipe-card animate-slide-up';
        recipeCard.style.animationDelay = `${index * 0.1}s`;
        
        recipeCard.innerHTML = `
          <div class="recipe-image">
            <span>${recipe.icon}</span>
            <div class="recipe-difficulty">${recipe.difficulty}</div>
          </div>
          <div class="recipe-info">
            <div class="recipe-country">${recipe.country.charAt(0).toUpperCase() + recipe.country.slice(1)}</div>
            <h3 class="recipe-title">${recipe.title}</h3>
            <p class="recipe-description">${recipe.description}</p>
            <div class="recipe-stats">
              <div class="stat-item">
                <i class="fas fa-clock"></i>
                <span>${recipe.prepTime + recipe.cookTime} min</span>
              </div>
              <div class="stat-item">
                <i class="fas fa-users"></i>
                <span>${recipe.servings} servings</span>
              </div>
              <div class="stat-item">
                <i class="fas fa-fire"></i>
                <span>${recipe.calories} cal</span>
              </div>
            </div>
            <div class="recipe-benefits">
              <div class="benefits-title">💚 Health Benefits</div>
              <div class="benefits-text">${recipe.benefits}</div>
            </div>
            <button class="try-recipe-btn" onclick="openRecipeModal('${recipe.id}')">
              <i class="fas fa-utensils"></i> Try This Recipe
            </button>
          </div>
        `;
        
        grid.appendChild(recipeCard);
      });
    }

    function openRecipeModal(recipeId) {
      const recipe = recipes.find(r => r.id === recipeId);
      if (!recipe) return;

      const modal = document.getElementById('recipeModal');
      const modalTitle = document.getElementById('modalTitle');
      const modalSubtitle = document.getElementById('modalSubtitle');
      const modalContent = document.getElementById('modalContent');

      modalTitle.textContent = recipe.title;
      modalSubtitle.textContent = recipe.description;

      let ingredientsHTML = '<ul class="ingredients-list">';
      recipe.ingredients.forEach(ingredient => {
        ingredientsHTML += `
          <li class="ingredient-item">
            <div class="ingredient-checkbox" onclick="toggleIngredient(this)"></div>
            <span>${ingredient}</span>
          </li>
        `;
      });
      ingredientsHTML += '</ul>';

      let instructionsHTML = '<ol class="instructions-list">';
      recipe.instructions.forEach(instruction => {
        instructionsHTML += `
          <li class="instruction-item">
            <div class="step-number"></div>
            <div class="instruction-text">${instruction}</div>
          </li>
        `;
      });
      instructionsHTML += '</ol>';

      let nutritionHTML = '<div class="nutrition-grid">';
      Object.entries(recipe.nutrition).forEach(([key, value]) => {
        const label = key.charAt(0).toUpperCase() + key.slice(1);
        nutritionHTML += `
          <div class="nutrition-item">
            <div class="nutrition-value">${value}</div>
            <div class="nutrition-label">${label}</div>
          </div>
        `;
      });
      nutritionHTML += '</div>';

      modalContent.innerHTML = `
        <div class="recipe-meta">
          <div class="meta-item">
            <div class="meta-value">${recipe.prepTime}</div>
            <div class="meta-label">Prep Time (min)</div>
          </div>
          <div class="meta-item">
            <div class="meta-value">${recipe.cookTime}</div>
            <div class="meta-label">Cook Time (min)</div>
          </div>
          <div class="meta-item">
            <div class="meta-value">${recipe.servings}</div>
            <div class="meta-label">Servings</div>
          </div>
          <div class="meta-item">
            <div class="meta-value">${recipe.calories}</div>
            <div class="meta-label">Calories</div>
          </div>
          <div class="meta-item">
            <div class="meta-value">${recipe.difficulty}</div>
            <div class="meta-label">Difficulty</div>
          </div>
        </div>

        <div class="ingredients-section">
          <h3 class="section-title">
            <i class="fas fa-list"></i> Ingredients
          </h3>
          ${ingredientsHTML}
        </div>

        <div class="instructions-section">
          <h3 class="section-title">
            <i class="fas fa-utensils"></i> Instructions
          </h3>
          ${instructionsHTML}
        </div>

        <div class="nutrition-info">
          <h3 style="color: var(--success-green); margin-bottom: 1rem;">
            <i class="fas fa-heart"></i> Nutritional Information (per serving)
          </h3>
          ${nutritionHTML}
        </div>
      `;

      modal.style.display = 'block';
      document.body.style.overflow = 'hidden';
    }

    function closeModal() {
      const modal = document.getElementById('recipeModal');
      modal.style.display = 'none';
      document.body.style.overflow = 'auto';
    }

    function toggleIngredient(checkbox) {
      checkbox.classList.toggle('checked');
      if (checkbox.classList.contains('checked')) {
        checkbox.innerHTML = '<i class="fas fa-check"></i>';
      } else {
        checkbox.innerHTML = '';
      }
    }

    function filterRecipes(country) {
      currentFilter = country;
      
      // Update active filter button
      document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.classList.remove('active');
      });
      document.querySelector(`[data-country="${country}"]`).classList.add('active');
      
      // Render filtered recipes
      renderRecipes(country);
    }

    // Event listeners
    document.addEventListener('DOMContentLoaded', function() {
      renderRecipes();
      
      // Add click listeners to filter buttons
      document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.addEventListener('click', function() {
          const country = this.getAttribute('data-country');
          filterRecipes(country);
        });
      });
    });

    // Close modal when clicking outside
    window.onclick = function(event) {
      const modal = document.getElementById('recipeModal');
      if (event.target === modal) {
        closeModal();
      }
    }

    // Keyboard navigation
    document.addEventListener('keydown', function(e) {
      if (e.key === 'Escape') {
        closeModal();
      }
    });

    // Animate cards on scroll
    const observerOptions = {
      threshold: 0.1,
      rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach((entry, index) => {
        if (entry.isIntersecting) {
          setTimeout(() => {
            entry.target.classList.add('animate-slide-up');
          }, index * 100);
        }
      });
    }, observerOptions);

    // Observe recipe cards when they're rendered
    function observeCards() {
      document.querySelectorAll('.recipe-card').forEach(card => {
        observer.observe(card);
      });
    }

    // Call observeCards after initial render
    setTimeout(observeCards, 100);