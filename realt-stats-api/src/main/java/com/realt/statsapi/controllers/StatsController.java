package com.realt.statsapi.controllers;

import com.realt.statsapi.controllers.dto.StatsDetailsResponse;
import com.realt.statsapi.controllers.dto.StatsResponse;
import com.realt.statsapi.data.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Example;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.util.Collection;

@Controller
@RequestMapping("stats")
@CrossOrigin(originPatterns = "*")
public class StatsController {

    private static final Logger log = LoggerFactory.getLogger(StatsController.class);

    @Autowired
    private StatsRepository statsRepository;

    @Autowired
    private StatsDetailsRepository statsDetailsRepository;

    @GetMapping("/daily")
    public @ResponseBody
    StatsResponse getDaily() {

        log.debug("Loading daily stats");
        Collection<DailyItem> data = statsRepository.findAll();
        log.info("Loaded daily stats: {}", data.size());

        return new StatsResponse(data.toArray(new DailyItem[0]));
    }

    @GetMapping("/daily/detailed")
    public @ResponseBody
    StatsDetailsResponse getDailyDetails() {

        log.debug("Loading detailed daily stats");
        Collection<DailyDetails> data = statsDetailsRepository.findAll();
        log.info("Loaded detailed daily stats: {}", data.size());

        return new StatsDetailsResponse(data.toArray(new DailyDetails[0]));
    }


    @GetMapping("/daily/detailed/{day}")
    public @ResponseBody StatsDetailsResponse getDailyDetails(@PathVariable String day) {

        log.debug("Loading daily stats for {}", day);
//        DailyDetails details = new DailyDetails();
//        details.setDay(date);
//        Example<DailyDetails> example = Example.of(details);
//        Collection<DailyDetails> data = statsDetailsRepository.findAll(example);
        Collection<DailyDetails> data = statsDetailsRepository.findByDay(day);
        if (data.isEmpty()){
            log.warn("Not found for {}", day);
            throw new ResponseStatusException(
                    HttpStatus.NOT_FOUND, "Daily Stats Not Found");
        }

        log.info("Loaded daily details for {}: {}", day, data.size());

        return new StatsDetailsResponse(data.toArray(new DailyDetails[0]));
    }
}