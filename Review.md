I've annotated code with comments so its easier to relate. Most of the issues are repetitive and seen throughout the codebase. I've annotated them as I navigated the code, but at a certain point it became too repetitive and there was no point in rehashing the same things over and over again. That doesnt mean classes not annotated are GOOD! It just means the same stuff applies everwhere.

The good thing is the fixes are going to be repetitive as well.

Biggest RED Flag: No Unit Testing and by that I mean real unit testing not just Integration Testing. Even if you added a Test project right now, you would only be able to write ITs. I dont recall seeing a single non-trivial method that's utable.

Get that one thing right and you'd be 5x better.

PS: Ive reviewed on reddit before but due to toxic and bad experiences I dont engage with it anymore. Just wanted to make sure you are serious and not a troll. 

## Few Assumptions
I'll assume the whole app was submitted by a professioanl (as a PR) and would expect production quality code, even though the nature and scope of work isnt too large. 

Convention Used:
- Red Flag (I would reject the PR)
- Yello Flag (Not outright rejection, but worth discussion - maybe create Tech Debt from it for future refactor)
- White Flag (Just a note/remark, otherwise fine)
- Green Flag (+1 - good job!)

## Positive

- [Green] Readme with image and few words explaining the basic setup and run is really appreciated.
- [Green] Using cross platform Avalonia.

## Negatives

- [White] No github actions: Not a deal breaker but github provided free CICD setup and integration and for a simple app like this its trivial to setup. For an Avalonia based app you can showcase Windows, Linux, Mac releases out of github actions. Also, a github action creates more trust as I can go to the action and download release from there. Its link is more trustworthy for a stranger than the link you advertize in your Readme.
- [Red] 0 Unit Test. Not even a basic empty Test prorject. Clear that testing was not even in the plan.
- [Yellow] Monolith project with everything clumped together, UI, business, Data. There are so many different way to strucuture e.g.:
  - FileOrganizer.App
  - FileOrganizer.Core
  - FileOrganizer.Data
