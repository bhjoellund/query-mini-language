Content Query Block Mini Language
=================================

### Purpose
The purpose of this document, is to describe the mini language used by content query blocks.
It is a simple expression language, that always evaluates to a boolean value, based on a number of simple rules.

Only developers should ever see this, as it is stored in the database, in a text field.

### Rationale
The reason for storing the query in a simple string is to able to store it succintly in the database. A more complex data model would be cumbersome, if not impossible, to store.

### Format
```javascript
<field_name> <operator> <value>[ <boolean_operator> <field_name> <operator> <value>]*
```

### Allowed field names
```javascript
author_id    // Author id (integer)
pub_date     // Mofibo publication date (datetime)
title        // Title (string)
kpi          // KPI (floating point)
publisher_id // Publisher id (integer)
language     // Book language (string)
type         // Book discriminator {Ebook, AudioBook} (string)
price        // M-price (floating point)
series_id    // Series id (integer)
bic_genre_id // BIC genre id (integer)
```

### Allowed operators
```javascript
=   // Equals
!=  // Does not equal
<   // Lesser than
>   // Greater than
<=  // Lesser than or equal to
>=  // Greater than or equal to
~=  // In (only applicable for lists)
```

### Allowed boolean operators
```javascript
&& // AND
```

### Allowed value types
All value types are required to be of the same type as that of the field they're compared against, with the exception of [lists](#lists)

##### String
Strings are delimited by &quot;. They can contain &quot; characters, if they are escaped with a backslash (\\)
```javascript
"string value"
```

##### Date and time
Date and time are always denoted as UTC
```javascript
/2015-04-09/
/2015-04-09 14:15:00/
```

##### Numbers
Numbers are split up into two subtypes, integers and floating point.

###### Integers
```javascript
1
42
```

###### Floating point
```javascript
2.0
3.14159265
2.71828
```

##### Lists
A list of values. All values in a list are required to be of the same type as the field they're compared against, and therefore all of the same type.
```javascript
(42)
(46 34 642)
(/2015-04-09/ /2015-04-11/)
("Isfolket" "Fiddy Shades")
```

### Examples
```javascript
author_id = 423
pub_date >= /2015-04-09/
kpi < 5.5
publisher_id = 324 && kpi < 5.5
language ~= ("dan" "eng")
```